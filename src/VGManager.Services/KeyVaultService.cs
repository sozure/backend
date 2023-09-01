using Azure.Security.KeyVault.Secrets;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using VGManager.Repository.Entities;
using VGManager.Repository.Interfaces;
using VGManager.Services.Interfaces;
using VGManager.Services.Models.Secrets;

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

    public async Task<SecretResultsModel> GetSecretsAsync(string secretFilter, CancellationToken cancellationToken = default)
    {
        var secretList = new List<SecretResultModel>();
        var secretsEntity = await _keyVaultConnectionRepository.GetSecrets(cancellationToken);
        var status = secretsEntity.Status;
        var secrets = secretsEntity?.Secrets ?? Enumerable.Empty<SecretEntity>();

        if (status == Status.Success)
        {
            var filteredSecrets = Filter(secrets, secretFilter);

            foreach (var filteredSecret in filteredSecrets)
            {
                if (filteredSecret is not null)
                {
                    var secretName = filteredSecret.Secret?.Name ?? string.Empty;
                    var secretValue = filteredSecret.Secret?.Value ?? string.Empty;

                    if(!string.IsNullOrEmpty(secretName) && !string.IsNullOrEmpty(secretValue))
                    {
                        secretList.Add(new()
                        {
                            SecretName = secretName,
                            SecretValue = secretValue
                        });
                    }
                }
            }

            return new()
            {
                Status = status,
                Results = secretList
            };
        }

        return new()
        {
            Status = status,
            Results = secretList
        };
    }

    public DeletedSecretResultsModel GetDeletedSecrets(string secretFilter, CancellationToken cancellationToken = default)
    {
        var secretList = new List<DeletedSecretResultModel>();
        var secretsEntity = _keyVaultConnectionRepository.GetDeletedSecrets(cancellationToken);
        var status = secretsEntity.Status;

        if (status == Status.Success)
        {
            var filteredSecrets = Filter(secretsEntity!.DeletedSecrets, secretFilter);

            foreach (var filteredSecret in filteredSecrets)
            {
                secretList.Add(new()
                {
                    SecretName = filteredSecret.Name,
                    DeletedOn = filteredSecret.DeletedOn
                });
            }
            return new()
            {
                Status = status,
                Results = secretList
            };
        }
        return new()
        {
            Status = status,
            Results = Enumerable.Empty<DeletedSecretResultModel>()
        };
    }

    public async Task<Status> DeleteAsync(string secretFilter, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Deleted secret key, value");
        var secretsResultModel = await _keyVaultConnectionRepository.GetSecrets(cancellationToken);
        var status = secretsResultModel.Status;
        
        if (status == Status.Success)
        {
            return await DeleteAsync(secretFilter, secretsResultModel, cancellationToken);
        }

        return status;
    }

    public async Task<Status> RecoverSecretAsync(string secretFilter, CancellationToken cancellationToken = default)
    {
        var deletedSecretsEntity = _keyVaultConnectionRepository.GetDeletedSecrets(cancellationToken);
        var status = deletedSecretsEntity.Status;

        if (status == Status.Success)
        {
            var filteredSecrets = Filter(deletedSecretsEntity.DeletedSecrets, secretFilter);
            var recoverCounter = 0;
            foreach (var secret in filteredSecrets)
            {
                var recoverStatus = await _keyVaultConnectionRepository.RecoverSecret(secret.Name, cancellationToken);
                if(recoverStatus == Status.Success)
                {
                    recoverCounter++;
                }
            }
            return recoverCounter == filteredSecrets.Count()? Status.Success: Status.Unknown;
        }
        return status;
    }

    private static IEnumerable<SecretEntity> Filter(IEnumerable<SecretEntity> keyVaultSecrets, string filter)
    {
        var regex = new Regex(filter);
        return keyVaultSecrets.Where(secret => regex.IsMatch(secret?.Secret?.Name ?? string.Empty)).ToList();
    }

    private static IEnumerable<DeletedSecret> Filter(IEnumerable<DeletedSecret> keyVaultSecrets, string filter)
    {
        var regex = new Regex(filter);
        return keyVaultSecrets.Where(secret => regex.IsMatch(secret?.Name ?? string.Empty)).ToList();
    }

    private async Task<Status> DeleteAsync(string secretFilter, SecretsEntity? secretsResultModel, CancellationToken cancellationToken)
    {
        var secrets = secretsResultModel?.Secrets ?? Enumerable.Empty<SecretEntity>();
        var filteredSecrets = Filter(secrets, secretFilter);
        var deletionCounter1 = 0;
        var deletionCounter2 = 0;

        foreach (var secret in filteredSecrets)
        {
            var secretName = secret?.Secret?.Name;
            var secretValue = secret?.Secret?.Value;
            if (secretName is not null && secretValue is not null)
            {
                deletionCounter1++;
                var deletionStatus = await _keyVaultConnectionRepository.DeleteSecret(secretName, cancellationToken);

                if (deletionStatus == Status.Success)
                {
                    Console.WriteLine($"{secretName}, {secretValue}");
                    deletionCounter2++;
                }
            }
        }

        if (deletionCounter1 != deletionCounter2)
        {
            return Status.Unknown;
        }

        return Status.Success;
    }
}
