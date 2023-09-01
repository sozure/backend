using VGManager.Services.Models.Secrets;

namespace VGManager.Services.Interfaces;

public interface IKeyVaultService
{
    void SetupConnectionRepository(string keyVaultName);
    Task<IEnumerable<SecretResultModel>> GetSecretsAsync(string secretFilter, CancellationToken cancellationToken = default);
    IEnumerable<DeletedSecretResultModel> GetDeletedSecrets(string secretFilter, CancellationToken cancellationToken = default);
    Task RecoverSecretAsync(string secretFilter, CancellationToken cancellationToken = default);
    Task DeleteAsync(string secretFilter, CancellationToken cancellationToken = default);
}
