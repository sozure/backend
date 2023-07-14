using Azure.Security.KeyVault.Secrets;
using System.Text.RegularExpressions;
using VGService.Repositories.Interfaces;
using VGService.Services.Interfaces;

namespace VGService.Services.Abstract_classes;

public abstract class KVCommand : IExecCommand
{
    protected readonly IKeyVaultConnectionRepository _connectionService;

    protected readonly string _secretFilter;

    public KVCommand(IKeyVaultConnectionRepository connectionService, string secretFilter)
    {
        _connectionService = connectionService;
        _secretFilter = secretFilter;
    }

    public abstract Task Execute();

    protected static IEnumerable<KeyVaultSecret> Filter(IEnumerable<KeyVaultSecret> keyVaultSecrets, string filter)
    {
        var regex = new Regex(filter);
        return keyVaultSecrets.Where(vg => regex.IsMatch(vg.Name));
    }
}
