using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using VGService.Repositories.Interfaces;

namespace VGService.Repositories;

public class KeyVaultConnectionRepository : IKeyVaultConnectionRepository
{

    private SecretClient _secretClient;
    public string KeyVaultName { get; set; }

    public async Task<KeyVaultSecret?> GetKeyVaultSecret(string name)
    {
        KeyVaultSecret result;
        try
        {
            result = await _secretClient.GetSecretAsync(name);
        }
        catch (Azure.RequestFailedException)
        {
            result = null!;
        }
        return result;
    }

    public async Task DeleteSecret(string name)
    {
        await _secretClient.StartDeleteSecretAsync(name);
    }

    public async Task<List<KeyVaultSecret?>> GetKeyVaultSecrets()
    {
        List<KeyVaultSecret> keyVaultSecrets = new();
        List<SecretProperties> secretProperties = _secretClient.GetPropertiesOfSecrets().ToList();

        var results = await Task.WhenAll(secretProperties.Select(p => GetKeyVaultSecret(p.Name)));

        return results.ToList();
    }

    public async Task AddKeyVaultSecret(Dictionary<string, string> parameters)
    {
        bool didWeRecover = false;
        string secretName = parameters["secretName"];
        string secretValue = parameters["secretValue"];
        var newSecret = new KeyVaultSecret(secretName, secretValue);
        List<DeletedSecret> deletedSecrets = _secretClient.GetDeletedSecrets().ToList();
        foreach (DeletedSecret deletedSecret in deletedSecrets)
        {
            if (deletedSecret.Name.Equals(secretName))
            {
                await _secretClient.StartRecoverDeletedSecretAsync(secretName);
                didWeRecover = true;
            }
        }
        if (!didWeRecover)
        {
            await _secretClient.SetSecretAsync(newSecret);
        }
    }

    public async Task RecoverSecret(string name)
    {
        await _secretClient.StartRecoverDeletedSecretAsync(name);
    }

    public async Task<IEnumerable<DeletedSecret>> GetDeletedSecretsAsync()
    {
        IEnumerable<DeletedSecret> deletedSecrets = _secretClient.GetDeletedSecrets().ToList();
        return deletedSecrets;
    }

    public void Setup(string keyVaultName)
    {
        var uri = new Uri($"https://{keyVaultName.ToLower()}.vault.azure.net/");
        var defaultazCred = new DefaultAzureCredential();
        _secretClient = new SecretClient(uri, defaultazCred);
        KeyVaultName = keyVaultName;
    }
}
