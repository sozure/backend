using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using VGManager.AzureAdapter.Entities;
using VGManager.AzureAdapter.Interfaces;
using VGManager.Services.Interfaces;
using VGManager.Services.Models.Secrets;

namespace VGManager.Services;

public class KeyVaultService : IKeyVaultService
{
    private readonly IKeyVaultAdapter _keyVaultConnectionRepository;
    private readonly ILogger _logger;

    public KeyVaultService(IKeyVaultAdapter keyVaultConnectionRepository, ILogger<KeyVaultService> logger)
    {
        _keyVaultConnectionRepository = keyVaultConnectionRepository;
        _logger = logger;
    }

    public void SetupConnectionRepository(SecretModel secretModel)
    {
        _keyVaultConnectionRepository.Setup(secretModel.KeyVaultName, secretModel.TenantId, secretModel.ClientId, secretModel.ClientSecret);
    }

    public async Task<SecretResultsModel> GetSecretsAsync(string secretFilter, CancellationToken cancellationToken = default)
    {
        var secretList = new List<SecretResultModel>();
        var secretsEntity = await _keyVaultConnectionRepository.GetSecretsAsync(cancellationToken);
        var status = secretsEntity.Status;
        var secrets = CollectSecrets(secretsEntity);

        if (status == Status.Success)
        {
            var filteredSecrets = Filter(secrets, secretFilter);

            foreach (var filteredSecret in filteredSecrets)
            {
                CollectSecrets(secretList, filteredSecret);
            }

            return GetResult(status, secretList);
        }
        return GetResult(status, secretList);
    }

    public async Task<Status> CopySecretsAsync(SecretCopyModel secretCopyModel, CancellationToken cancellationToken = default)
    {
        _keyVaultConnectionRepository.Setup(
            secretCopyModel.FromKeyVault,
            secretCopyModel.TenantId,
            secretCopyModel.ClientId,
            secretCopyModel.ClientSecret
            );

        var fromSecrets = await _keyVaultConnectionRepository.GetAllAsync(cancellationToken);

        _keyVaultConnectionRepository.Setup(
            secretCopyModel.ToKeyVault,
            secretCopyModel.TenantId,
            secretCopyModel.ClientId,
            secretCopyModel.ClientSecret
            );

        var toSecrets = await _keyVaultConnectionRepository.GetAllAsync(cancellationToken);

        foreach (var secret in fromSecrets)
        {
            var parameters = ParametersBuilder(secret, toSecrets, secretCopyModel.overrideSecret);
            var partialStatus = await _keyVaultConnectionRepository.AddKeyVaultSecretAsync(parameters, cancellationToken);

            if (partialStatus != Status.Success)
            {
                return partialStatus;
            }
        }

        return Status.Success;
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
            return GetResult(status, secretList);
        }
        return GetResult(status);
    }


    public async Task<Status> DeleteAsync(string secretFilter, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Deleted secret key, value");
        var secretsResultModel = await _keyVaultConnectionRepository.GetSecretsAsync(cancellationToken);
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
                var recoverStatus = await _keyVaultConnectionRepository.RecoverSecretAsync(secret.Name, cancellationToken);
                if (recoverStatus == Status.Success)
                {
                    recoverCounter++;
                }
            }
            return recoverCounter == filteredSecrets.Count() ? Status.Success : Status.Unknown;
        }
        return status;
    }

    private static IEnumerable<SecretEntity> Filter(IEnumerable<SecretEntity> keyVaultSecrets, string filter)
    {
        var regex = new Regex(filter.ToLower());
        return keyVaultSecrets.Where(secret => regex.IsMatch(secret?.Secret?.Name.ToLower() ?? string.Empty)).ToList();
    }

    private static IEnumerable<DeletedSecret> Filter(IEnumerable<DeletedSecret> keyVaultSecrets, string filter)
    {
        var regex = new Regex(filter.ToLower());
        return keyVaultSecrets.Where(secret => regex.IsMatch(secret?.Name.ToLower() ?? string.Empty)).ToList();
    }

    private static Dictionary<string, string> ParametersBuilder(
        KeyVaultSecret keyVaultSecret,
        IEnumerable<KeyVaultSecret> toKeyVaultSecrets,
        bool overrideSecret
        )
    {
        var parameters = new Dictionary<string, string>
        {
            ["secretName"] = keyVaultSecret.Name
        };

        var toKeyVaultSecret = toKeyVaultSecrets.FirstOrDefault(kv => kv.Name.Equals(keyVaultSecret.Name));
        parameters["secretValue"] = overrideSecret || toKeyVaultSecret is null ? keyVaultSecret.Value : toKeyVaultSecret.Value;

        return parameters;
    }

    private async Task<Status> DeleteAsync(string secretFilter, SecretsEntity? secretsResultModel, CancellationToken cancellationToken)
    {
        var secrets = CollectSecrets(secretsResultModel);
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
                var deletionStatus = await _keyVaultConnectionRepository.DeleteSecretAsync(secretName, cancellationToken);

                if (deletionStatus == Status.Success)
                {
                    _logger.LogInformation("{secretName}, {secretValue}", secretName, secretValue);
                    deletionCounter2++;
                }
            }
        }
        return deletionCounter1 == deletionCounter2 ? Status.Success : Status.Unknown;
    }

    private static IEnumerable<SecretEntity> CollectSecrets(SecretsEntity? secretsResultModel)
    {
        var result = new List<SecretEntity>();
        if (secretsResultModel is null)
        {
            return Enumerable.Empty<SecretEntity>();
        }

        foreach (var secret in secretsResultModel.Secrets)
        {
            if (secret is not null)
            {
                result.Add(secret);
            }
        }

        return result;
    }

    private static void CollectSecrets(List<SecretResultModel> secretList, SecretEntity? filteredSecret)
    {
        if (filteredSecret is not null)
        {
            var secretName = filteredSecret.Secret?.Name ?? string.Empty;
            var secretValue = filteredSecret.Secret?.Value ?? string.Empty;

            if (!string.IsNullOrEmpty(secretName) && !string.IsNullOrEmpty(secretValue))
            {
                secretList.Add(new()
                {
                    SecretName = secretName,
                    SecretValue = secretValue
                });
            }
        }
    }

    private static DeletedSecretResultsModel GetResult(Status status)
    {
        return new()
        {
            Status = status,
            DeletedSecrets = Enumerable.Empty<DeletedSecretResultModel>()
        };
    }

    private static DeletedSecretResultsModel GetResult(Status status, IEnumerable<DeletedSecretResultModel> secretList)
    {
        return new()
        {
            Status = status,
            DeletedSecrets = secretList
        };
    }

    private static SecretResultsModel GetResult(Status status, IEnumerable<SecretResultModel> secretList)
    {
        return new()
        {
            Status = status,
            Secrets = secretList
        };
    }
}
