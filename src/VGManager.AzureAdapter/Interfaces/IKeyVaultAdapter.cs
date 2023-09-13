using Azure.Security.KeyVault.Secrets;
using VGManager.AzureAdapter.Entities;

namespace VGManager.AzureAdapter.Interfaces;

public interface IKeyVaultAdapter
{
    Task<Status> AddKeyVaultSecretAsync(Dictionary<string, string> parameters, CancellationToken cancellationToken = default);
    Task<Status> DeleteSecretAsync(string name, CancellationToken cancellationToken = default);
    Task<SecretEntity> GetSecretAsync(string name, CancellationToken cancellationToken = default);
    Task<SecretsEntity> GetSecretsAsync(CancellationToken cancellationToken = default);
    Task<Status> RecoverSecretAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<KeyVaultSecret>> GetAllAsync(CancellationToken cancellationToken);
    DeletedSecretsEntity GetDeletedSecrets(CancellationToken cancellationToken = default);
    public void Setup(string keyVaultName, string tenantId, string clientId, string clientSecret);
}
