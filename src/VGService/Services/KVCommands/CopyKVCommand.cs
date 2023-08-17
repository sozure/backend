using Azure.Security.KeyVault.Secrets;
using VGService.Repositories.Interfaces;
using VGService.Services.Abstract_classes;

namespace VGService.Services.KVCommands;

public class CopyKVCommand : KVCommand
{
    private readonly IKeyVaultConnectionRepository _destinationKeyVault;
    private readonly bool _dryRun;
    private readonly bool _overWrite;

    public CopyKVCommand(IKeyVaultConnectionRepository connectionService, string secretFilter, IKeyVaultConnectionRepository destinationKeyVault, bool overWrite, bool dryRun)
        : base(connectionService, secretFilter)
    {
        _destinationKeyVault = destinationKeyVault;
        _dryRun = dryRun;
        _overWrite = overWrite;
    }

    public override async Task Execute()
    {
        var fromKeyVaultSecrets = await _connectionService.GetKeyVaultSecrets();

        var filteredFromList = Filter(fromKeyVaultSecrets!, _secretFilter);

        foreach (var keyVaultSecret in filteredFromList)
        {
            var copyIsNeeded = false;
            var secretFromDestinationKeyVault = await _destinationKeyVault.GetKeyVaultSecret(keyVaultSecret.Name);
            var parameters = SetParametersForCopy(keyVaultSecret, ref copyIsNeeded, secretFromDestinationKeyVault!);

            await AddSecretToDestionation(keyVaultSecret, copyIsNeeded, parameters);
        }
    }

    private Dictionary<string, string> SetParametersForCopy(KeyVaultSecret keyVaultSecret, ref bool copyIsNeeded, KeyVaultSecret secretFromDestinationKeyVault)
    {
        Dictionary<string, string> parameters = null!;

        if (secretFromDestinationKeyVault != null)
        {
            if (_overWrite)
            {
                parameters = new Dictionary<string, string>
                {
                    ["keyVaultName"] = keyVaultSecret.Name,
                    ["keyVaultValue"] = keyVaultSecret.Value
                };
                copyIsNeeded = true;
            }
        }
        else
        {
            parameters = new Dictionary<string, string>
            {
                ["keyVaultName"] = keyVaultSecret.Name,
                ["keyVaultValue"] = keyVaultSecret.Value
            };
            copyIsNeeded = true;
        }
        return parameters;
    }

    private async Task AddSecretToDestionation(KeyVaultSecret keyVaultSecret, bool copyIsNeeded, Dictionary<string, string> parameters)
    {
        if (!_dryRun)
        {
            if (copyIsNeeded)
            {
                await _destinationKeyVault.AddKeyVaultSecret(parameters);
                Console.WriteLine($"{keyVaultSecret.Name} was added to {_destinationKeyVault.KeyVaultName}.");
            }
        }
        else
        {
            Console.WriteLine($"{keyVaultSecret.Name} was added to {_destinationKeyVault.KeyVaultName} in dry run mode.");
        }
    }
}
