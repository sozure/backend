using VGManager.AzureAdapter.Entities;
using VGManager.Services.Models.Secrets.Requests;
using VGManager.Services.Models.Secrets.Results;

namespace VGManager.Services.Interfaces;

public interface IKeyVaultService
{
    void SetupConnectionRepository(SecretModel secretModel);
    Task<IEnumerable<string>> GetKeyVaultsAsync(string tenantId, string clientId, string clientSecret, CancellationToken cancellationToken = default);
    Task<SecretResults> GetSecretsAsync(string secretFilter, CancellationToken cancellationToken = default);
    DeletedSecretResults GetDeletedSecrets(string secretFilter, CancellationToken cancellationToken = default);
    Task<AdapterStatus> RecoverSecretAsync(string secretFilter, string userName, CancellationToken cancellationToken = default);
    Task<AdapterStatus> DeleteAsync(string secretFilter, string userName, CancellationToken cancellationToken = default);
    Task<AdapterStatus> CopySecretsAsync(SecretCopyModel secretCopyModel, CancellationToken cancellationToken = default);
}
