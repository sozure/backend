using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Serilog;
using VGManager.Repository.Entities;
using VGManager.Repository.Interfaces;

namespace VGManager.Repository;

public class KeyVaultConnectionRepository : IKeyVaultConnectionRepository
{
    private SecretClient _secretClient = null!;
    public string KeyVaultName { get; set; } = null!;
    private readonly ILogger _logger = Log.ForContext<KeyVaultConnectionRepository>();

    public void Setup(string keyVaultName)
    {
        var uri = new Uri($"https://{keyVaultName.ToLower()}.vault.azure.net/");
        var defaultazCred = new DefaultAzureCredential();
        _secretClient = new SecretClient(uri, defaultazCred);
        KeyVaultName = keyVaultName;
    }

    public async Task<SecretEntity> GetSecret(string name, CancellationToken cancellationToken = default)
    {
        KeyVaultSecret result;
        try
        {
            result = await _secretClient.GetSecretAsync(name, cancellationToken: cancellationToken);
            return GetSecretResult(result);
        }
        catch (Azure.RequestFailedException ex)
        {
            var status = Status.Unknown;
            _logger.Error(ex, "Couldn't get secret. Status: {status}.", status);
            return GetSecretResult(status);
        }
        catch (Exception ex)
        {
            var status = Status.Unknown;
            _logger.Error(ex, "Couldn't get secret. Status: {status}.", status);
            return GetSecretResult(status);
        }
    }

    public async Task<Status> DeleteSecret(string name, CancellationToken cancellationToken = default)
    {
        try
        {
            await _secretClient.StartDeleteSecretAsync(name, cancellationToken);
            return Status.Success;
        }
        catch (Exception ex)
        {
            var status = Status.Unknown;
            _logger.Error(ex, "Couldn't delete secret. Status: {status}.", status);
            return status;
        }
    }

    public async Task<SecretsEntity> GetSecrets(CancellationToken cancellationToken = default)
    {
        try
        {
            var secretProperties = _secretClient.GetPropertiesOfSecrets(cancellationToken).ToList();
            var results = await Task.WhenAll(secretProperties.Select(p => GetSecret(p.Name)));

            if(results is null)
            {
                return GetSecretsResult(Status.Unknown);
            }
            return GetSecretsResult(results.ToList());

        } catch (Exception ex)
        {
            var status = Status.Unknown;
            _logger.Error(ex, "Couldn't get secrets. Status: {status}.", status);
            return GetSecretsResult(status);
        }
    }

    public async Task<Status> AddKeyVaultSecret(Dictionary<string, string> parameters, CancellationToken cancellationToken = default)
    {
        try
        {
            var didWeRecover = false;
            var secretName = parameters["secretName"];
            var secretValue = parameters["secretValue"];
            var newSecret = new KeyVaultSecret(secretName, secretValue);
            var deletedSecrets = _secretClient.GetDeletedSecrets(cancellationToken).ToList();

            foreach (var deletedSecret in deletedSecrets)
            {
                if (deletedSecret.Name.Equals(secretName))
                {
                    await _secretClient.StartRecoverDeletedSecretAsync(secretName, cancellationToken);
                    didWeRecover = true;
                }
            }

            if (!didWeRecover)
            {
                await _secretClient.SetSecretAsync(newSecret, cancellationToken);
            }

            return Status.Success;
        }
        catch (Exception ex)
        {
            var status = Status.Unknown;
            _logger.Error(ex, "Couldn't add secret. Status: {status}.", status);
            return status;
        }
    }

    public async Task<Status> RecoverSecret(string name, CancellationToken cancellationToken = default)
    {
        try
        {
            await _secretClient.StartRecoverDeletedSecretAsync(name, cancellationToken);
            return Status.Success;
        }
        catch (Exception ex) 
        {
            var status = Status.Unknown;
            _logger.Error(ex, "Couldn't recover secret. Status: {status}.", status);
            return status;
        }
    }

    public DeletedSecretsEntity GetDeletedSecrets(CancellationToken cancellationToken = default)
    {
        try
        {
            var deletedSecrets = _secretClient.GetDeletedSecrets(cancellationToken).ToList();
            return GetDeletedSecretsResult(deletedSecrets);
        }
        catch (Exception ex)
        {
            var status = Status.Unknown;
            _logger.Error(ex, "Couldn't get deleted secrets. Status: {status}.", status);
            return GetDeletedSecretsResult(status);
        }
    }

    private static DeletedSecretsEntity GetDeletedSecretsResult(Status status)
    {
        return new()
        {
            Status = status,
            DeletedSecrets = Enumerable.Empty<DeletedSecret>()
        };
    }

    private static DeletedSecretsEntity GetDeletedSecretsResult(IEnumerable<DeletedSecret> deletedSecrets)
    {
        return new()
        {
            Status = Status.Success,
            DeletedSecrets = deletedSecrets
        };
    }

    private static SecretsEntity GetSecretsResult(IEnumerable<SecretEntity?> secrets)
    {
        return new()
        {
            Status = Status.Success,
            Secrets = secrets
        };
    }

    private static SecretsEntity GetSecretsResult(Status status)
    {
        return new()
        {
            Status = status,
            Secrets = Enumerable.Empty<SecretEntity>()
        };
    }

    private static SecretEntity GetSecretResult(KeyVaultSecret result)
    {
        return new()
        {
            Status = Status.Success,
            Secret = result
        };
    }

    private static SecretEntity GetSecretResult(Status status)
    {
        return new()
        {
            Status = status,
            Secret = null!
        };
    }
}