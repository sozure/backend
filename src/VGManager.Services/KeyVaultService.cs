using Azure.Security.KeyVault.Secrets;
using System.Text.RegularExpressions;
using VGManager.Repository.Interfaces;
using VGManager.Services.Interfaces;
using VGManager.Services.Models;

namespace VGManager.Services;

public class KeyVaultService : IKeyVaultService
{
    private readonly IKeyVaultConnectionRepository _keyVaultConnectionRepository;

    public KeyVaultService(IKeyVaultConnectionRepository keyVaultConnectionRepository)
    {
        _keyVaultConnectionRepository = keyVaultConnectionRepository;
    }

    public void SetupConnectionRepository(string keyVaultName)
    {
        _keyVaultConnectionRepository.Setup(keyVaultName);
    }

    public async Task<IEnumerable<MatchedSecret>> GetSecretsAsync(string secretFilter, CancellationToken cancellationToken = default)
    {
        var secretList = new List<MatchedSecret>();
        var secrets = await _keyVaultConnectionRepository.GetKeyVaultSecrets(cancellationToken);
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

    public IEnumerable<MatchedDeletedSecret> GetDeletedSecrets(string secretFilter, CancellationToken cancellationToken = default)
    {
        var secretList = new List<MatchedDeletedSecret>();
        var secrets = _keyVaultConnectionRepository.GetDeletedSecrets(cancellationToken);
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

    public async Task DeleteAsync(string secretFilter, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Deleted secret key, value");
        var secrets = await _keyVaultConnectionRepository.GetKeyVaultSecrets(cancellationToken);
        var filteredSecrets = Filter(secrets!, secretFilter);

        foreach (var secret in filteredSecrets)
        {
            await _keyVaultConnectionRepository.DeleteSecret(secret.Name, cancellationToken);
            Console.WriteLine($"{secret.Name}, {secret.Value}");
        }
    }

    public async Task RecoverSecretAsync(string secretFilter, CancellationToken cancellationToken = default)
    {
        var deletedSecrets = _keyVaultConnectionRepository.GetDeletedSecrets(cancellationToken);
        var filteredSecrets = Filter(deletedSecrets, secretFilter);

        foreach (var secret in filteredSecrets)
        {
            await _keyVaultConnectionRepository.RecoverSecret(secret.Name, cancellationToken);
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
