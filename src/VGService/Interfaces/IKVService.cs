using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VGService.Model;
using VGService.Repositories.Interfaces;

namespace VGService.Interfaces;

public interface IKVService
{
    public Task<IEnumerable<MatchedSecret>> GetSecretsAsync(IKeyVaultConnectionRepository connectionService, string secretFilter);
    public Task<IEnumerable<MatchedDeletedSecret>> GetDeletedSecretsAsync(IKeyVaultConnectionRepository connectionService, string secretFilter);

    public Task RecoverSecretAsync(IKeyVaultConnectionRepository connectionService, string secretFilter);

    public Task DeleteAsync(IKeyVaultConnectionRepository connectionService, string secretFilter);
}
