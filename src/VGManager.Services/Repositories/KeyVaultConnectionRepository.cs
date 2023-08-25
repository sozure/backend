using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using VGManager.Services.Repositories.Interfaces;

namespace VGManager.Services.Repositories;

public class KeyVaultConnectionRepository : IKeyVaultConnectionRepository
{
    private SecretClient _secretClient = null!;
    public string KeyVaultName { get; set; } = null!;

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
        var secretProperties = _secretClient.GetPropertiesOfSecrets().ToList();
        var results = await Task.WhenAll(secretProperties.Select(p => GetKeyVaultSecret(p.Name)));
        return results.ToList();
    }

    public async Task AddKeyVaultSecret(Dictionary<string, string> parameters)
    {
        var didWeRecover = false;
        var secretName = parameters["secretName"];
        var secretValue = parameters["secretValue"];
        var newSecret = new KeyVaultSecret(secretName, secretValue);
        var deletedSecrets = _secretClient.GetDeletedSecrets().ToList();

        foreach (var deletedSecret in deletedSecrets)
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

    public IEnumerable<DeletedSecret> GetDeletedSecrets()
    {
        return _secretClient.GetDeletedSecrets().ToList();
    }

    public void Setup(string keyVaultName)
    {
        var uri = new Uri($"https://{keyVaultName.ToLower()}.vault.azure.net/");
        var defaultazCred = new DefaultAzureCredential();
        _secretClient = new SecretClient(uri, defaultazCred);
        KeyVaultName = keyVaultName;
    }

    public Task<IEnumerable<DeletedSecret>> GetDeletedSecretsAsync()
    {
        throw new NotImplementedException();
    }
}
