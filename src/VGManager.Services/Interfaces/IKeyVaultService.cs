using VGManager.AzureAdapter.Entities;
using VGManager.Services.Models.Secrets.Requests;
using VGManager.Services.Models.Secrets.Results;

namespace VGManager.Services.Interfaces;

public interface IKeyVaultService
{
    void SetupConnectionRepository(SecretModel secretModel);
    Task<SecretResults> GetSecretsAsync(string secretFilter, CancellationToken cancellationToken = default);
    DeletedSecretResults GetDeletedSecrets(string secretFilter, CancellationToken cancellationToken = default);
    Task<Status> RecoverSecretAsync(string secretFilter, CancellationToken cancellationToken = default);
    Task<Status> DeleteAsync(string secretFilter, CancellationToken cancellationToken = default);
    Task<Status> CopySecretsAsync(SecretCopyModel secretCopyModel, CancellationToken cancellationToken = default);
}
