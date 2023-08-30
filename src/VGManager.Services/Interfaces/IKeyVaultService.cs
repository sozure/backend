using VGManager.Services.Models.MatchedModels;

namespace VGManager.Services.Interfaces;

public interface IKeyVaultService
{
    void SetupConnectionRepository(string keyVaultName);
    Task<IEnumerable<MatchedSecret>> GetSecretsAsync(string secretFilter, CancellationToken cancellationToken = default);
    IEnumerable<MatchedDeletedSecret> GetDeletedSecrets(string secretFilter, CancellationToken cancellationToken = default);
    Task RecoverSecretAsync(string secretFilter, CancellationToken cancellationToken = default);
    Task DeleteAsync(string secretFilter, CancellationToken cancellationToken = default);
}
