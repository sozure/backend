using Azure.Security.KeyVault.Secrets;

namespace VGService.Repositories.Interfaces;

public interface IKeyVaultConnectionRepository
{
    public string KeyVaultName { get; }
    Task AddKeyVaultSecret(Dictionary<string, string> parameters);
    Task DeleteSecret(string name);
    Task<KeyVaultSecret?> GetKeyVaultSecret(string name);
    Task<List<KeyVaultSecret?>> GetKeyVaultSecrets();
    Task RecoverSecret(string name);
    Task<IEnumerable<DeletedSecret>> GetDeletedSecretsAsync();
    public void Setup(string keyVaultName);
}
