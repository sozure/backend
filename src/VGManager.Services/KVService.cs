using Azure.Security.KeyVault.Secrets;
using System.Text.RegularExpressions;
using VGManager.Repository.Interfaces;
using VGManager.Services.Interfaces;
using VGManager.Services.Model;

namespace VGManager.Services;

public class KVService : IKVService
{
    private readonly IKeyVaultConnectionRepository _keyVaultConnectionRepository;

    public void SetupConnectionRepository(string keyVaultName)
    {
        _keyVaultConnectionRepository.Setup(keyVaultName);
    }

    public async Task<IEnumerable<MatchedSecret>> GetSecretsAsync(string secretFilter)
    {
        var secretList = new List<MatchedSecret>();
        var secrets = await _keyVaultConnectionRepository.GetKeyVaultSecrets();
        var filteredSecrets = Filter(secrets!, secretFilter);

        foreach (var filteredSecret in filteredSecrets)
        {
            if (filteredSecret is not null)
            {
                secretList.Add(new()
                {
                    SecretName = filteredSecret.Name, 
                    SecretValue = filteredSecret.Value
                });
            }
        }

        return secretList;
    }

    public async Task<IEnumerable<MatchedDeletedSecret>> GetDeletedSecretsAsync(string secretFilter)
    {
        var secretList = new List<MatchedDeletedSecret>();
        var secrets = await _keyVaultConnectionRepository.GetDeletedSecretsAsync();
        var filteredSecrets = Filter(secrets!, secretFilter);

        foreach (var filteredSecret in filteredSecrets)
        {
            secretList.Add(new() 
            {
                SecretName = filteredSecret.Name,
                DeletedOn = filteredSecret.DeletedOn
            });
        }
        return secretList;
    }

    public async Task DeleteAsync(string secretFilter)
    {
        Console.WriteLine("Deleted secret key, value");
        var secrets = await _keyVaultConnectionRepository.GetKeyVaultSecrets();
        var filteredSecrets = Filter(secrets!, secretFilter);

        foreach (var secret in filteredSecrets)
        {
            await _keyVaultConnectionRepository.DeleteSecret(secret.Name);
            Console.WriteLine($"{secret.Name}, {secret.Value}");
        }
    }

    public async Task RecoverSecretAsync(string secretFilter)
    {
        var deletedSecrets = await _keyVaultConnectionRepository.GetDeletedSecretsAsync();
        var filteredSecrets = Filter(deletedSecrets, secretFilter);

        foreach (var secret in filteredSecrets)
        {
            await _keyVaultConnectionRepository.RecoverSecret(secret.Name);
        }
    }

    protected static IEnumerable<KeyVaultSecret> Filter(IEnumerable<KeyVaultSecret> keyVaultSecrets, string filter)
    {
        var regex = new Regex(filter);
        return keyVaultSecrets.Where(secret => regex.IsMatch(secret?.Name ?? string.Empty)).ToList();
    }

    protected static IEnumerable<DeletedSecret> Filter(IEnumerable<DeletedSecret> keyVaultSecrets, string filter)
    {
        var regex = new Regex(filter);
        return keyVaultSecrets.Where(secret => regex.IsMatch(secret?.Name ?? string.Empty)).ToList();
    }
}
