using Azure.Security.KeyVault.Secrets;

namespace VGManager.Repository.Interfaces;

public interface IKeyVaultConnectionRepository
{
    Task AddKeyVaultSecret(Dictionary<string, string> parameters, CancellationToken cancellationToken = default);
    Task DeleteSecret(string name, CancellationToken cancellationToken = default);
    Task<KeyVaultSecret?> GetKeyVaultSecret(string name, CancellationToken cancellationToken = default);
    Task<List<KeyVaultSecret?>> GetKeyVaultSecrets(CancellationToken cancellationToken = default);
    Task RecoverSecret(string name, CancellationToken cancellationToken = default);
    IEnumerable<DeletedSecret> GetDeletedSecrets(CancellationToken cancellationToken = default);
    public void Setup(string keyVaultName);
}
