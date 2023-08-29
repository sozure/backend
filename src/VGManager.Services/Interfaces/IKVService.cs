using VGManager.Services.Model;

namespace VGManager.Services.Interfaces;

public interface IKVService
{
    void SetupConnectionRepository(string keyVaultName);
    Task<IEnumerable<MatchedSecret>> GetSecretsAsync(string secretFilter);
    Task<IEnumerable<MatchedDeletedSecret>> GetDeletedSecretsAsync(string secretFilter);
    Task RecoverSecretAsync(string secretFilter);
    Task DeleteAsync(string secretFilter);
}
