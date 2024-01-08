using Azure.Security.KeyVault.Secrets;
using VGManager.AzureAdapter.Entities;
using VGManager.Models;

namespace VGManager.AzureAdapter.Interfaces;

public interface IKeyVaultAdapter
{
    Task<(string?, IEnumerable<string>)> GetKeyVaultsAsync(
        string tenantId,
        string clientId,
        string clientSecret,
        CancellationToken cancellationToken = default
        );
    Task<AdapterStatus> AddKeyVaultSecretAsync(Dictionary<string, string> parameters, CancellationToken cancellationToken = default);
    Task<AdapterStatus> DeleteSecretAsync(string name, CancellationToken cancellationToken = default);
    Task<SecretEntity> GetSecretAsync(string name, CancellationToken cancellationToken = default);
    Task<AdapterResponseModel<IEnumerable<SecretEntity>>> GetSecretsAsync(CancellationToken cancellationToken = default);
    Task<AdapterStatus> RecoverSecretAsync(string name, CancellationToken cancellationToken = default);
    Task<IEnumerable<KeyVaultSecret>> GetAllAsync(CancellationToken cancellationToken);
    AdapterResponseModel<IEnumerable<DeletedSecret>> GetDeletedSecrets(CancellationToken cancellationToken = default);
    public void Setup(string keyVaultName, string tenantId, string clientId, string clientSecret);
}
