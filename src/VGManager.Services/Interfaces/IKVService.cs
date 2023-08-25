using VGManager.Services.Model;
using VGManager.Services.Repositories.Interfaces;

namespace VGManager.Services.Interfaces;

public interface IKVService
{
    public Task<IEnumerable<MatchedSecret>> GetSecretsAsync(IKeyVaultConnectionRepository connectionService, string secretFilter);
    public Task<IEnumerable<MatchedDeletedSecret>> GetDeletedSecretsAsync(IKeyVaultConnectionRepository connectionService, string secretFilter);
    public Task RecoverSecretAsync(IKeyVaultConnectionRepository connectionService, string secretFilter);
    public Task DeleteAsync(IKeyVaultConnectionRepository connectionService, string secretFilter);
}
