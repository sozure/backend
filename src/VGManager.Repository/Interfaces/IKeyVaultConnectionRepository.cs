using Azure.Security.KeyVault.Secrets;
using VGManager.Repository.Entities;

namespace VGManager.Repository.Interfaces;

public interface IKeyVaultConnectionRepository
{
    Task<Status> AddKeyVaultSecret(Dictionary<string, string> parameters, CancellationToken cancellationToken = default);
    Task<Status> DeleteSecret(string name, CancellationToken cancellationToken = default);
    Task<SecretEntity> GetSecret(string name, CancellationToken cancellationToken = default);
    Task<SecretsEntity> GetSecrets(CancellationToken cancellationToken = default);
    Task<Status> RecoverSecret(string name, CancellationToken cancellationToken = default);
    DeletedSecretsEntity GetDeletedSecrets(CancellationToken cancellationToken = default);
    public void Setup(string keyVaultName);
}
