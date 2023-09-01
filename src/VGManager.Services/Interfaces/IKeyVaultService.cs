using VGManager.Repository.Entities;
using VGManager.Services.Models.Secrets;

namespace VGManager.Services.Interfaces;

public interface IKeyVaultService
{
    void SetupConnectionRepository(string keyVaultName);
    Task<SecretResultsModel> GetSecretsAsync(string secretFilter, CancellationToken cancellationToken = default);
    DeletedSecretResultsModel GetDeletedSecrets(string secretFilter, CancellationToken cancellationToken = default);
    Task<Status> RecoverSecretAsync(string secretFilter, CancellationToken cancellationToken = default);
    Task<Status> DeleteAsync(string secretFilter, CancellationToken cancellationToken = default);
}
