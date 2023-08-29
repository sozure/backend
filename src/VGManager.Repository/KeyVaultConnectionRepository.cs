using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System.Threading;
using VGManager.Repository.Interfaces;

namespace VGManager.Repository;

public class KeyVaultConnectionRepository : IKeyVaultConnectionRepository
{
    private SecretClient _secretClient = null!;
    public string KeyVaultName { get; set; } = null!;

    public async Task<KeyVaultSecret?> GetKeyVaultSecret(string name, CancellationToken cancellationToken = default)
    {
        KeyVaultSecret result;
        try
        {
            result = await _secretClient.GetSecretAsync(name, cancellationToken: cancellationToken);
        }
        catch (Azure.RequestFailedException)
        {
            result = null!;
        }
        return result;
    }

    public async Task DeleteSecret(string name, CancellationToken cancellationToken = default)
    {
        await _secretClient.StartDeleteSecretAsync(name, cancellationToken);
    }

    public async Task<List<KeyVaultSecret?>> GetKeyVaultSecrets(CancellationToken cancellationToken = default)
    {
        var secretProperties = _secretClient.GetPropertiesOfSecrets(cancellationToken).ToList();
        var results = await Task.WhenAll(secretProperties.Select(p => GetKeyVaultSecret(p.Name)));
        return results.ToList();
    }

    public async Task AddKeyVaultSecret(Dictionary<string, string> parameters, CancellationToken cancellationToken = default)
    {
        var didWeRecover = false;
        var secretName = parameters["secretName"];
        var secretValue = parameters["secretValue"];
        var newSecret = new KeyVaultSecret(secretName, secretValue);
        var deletedSecrets = _secretClient.GetDeletedSecrets(cancellationToken).ToList();

        foreach (var deletedSecret in deletedSecrets)
        {
            if (deletedSecret.Name.Equals(secretName))
            {
                await _secretClient.StartRecoverDeletedSecretAsync(secretName, cancellationToken);
                didWeRecover = true;
            }
        }

        if (!didWeRecover)
        {
            await _secretClient.SetSecretAsync(newSecret, cancellationToken);
        }
    }

    public async Task RecoverSecret(string name, CancellationToken cancellationToken = default)
    {
        await _secretClient.StartRecoverDeletedSecretAsync(name, cancellationToken);
    }

    public IEnumerable<DeletedSecret> GetDeletedSecrets(CancellationToken cancellationToken = default)
    {
        return _secretClient.GetDeletedSecrets(cancellationToken).ToList();
    }

    public void Setup(string keyVaultName)
    {
        var uri = new Uri($"https://{keyVaultName.ToLower()}.vault.azure.net/");
        var defaultazCred = new DefaultAzureCredential();
        _secretClient = new SecretClient(uri, defaultazCred);
        KeyVaultName = keyVaultName;
    }
}