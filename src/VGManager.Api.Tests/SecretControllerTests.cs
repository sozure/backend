using AutoMapper;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VGManager.Api.Controllers;
using VGManager.Api.MapperProfiles;
using VGManager.Api.Secret.Request;
using VGManager.Api.Secrets.Response;
using VGManager.AzureAdapter.Entities;
using VGManager.AzureAdapter.Interfaces;
using VGManager.Services;
using VGManager.Services.Interfaces;

namespace VGManager.Api.Tests;

[TestFixture]
public class SecretControllerTests
{
    private SecretController _controller;
    private IKeyVaultService _keyVaultService;
    private Mock<IKeyVaultAdapter> _adapter;

    [SetUp]
    public void Setup()
    {
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(SecretProfile));
        });

        var mapper = mapperConfiguration.CreateMapper();

        _adapter = new(MockBehavior.Strict);
        var loggerMock = new Mock<ILogger<KeyVaultService>>();

        _keyVaultService = new KeyVaultService(_adapter.Object, loggerMock.Object);
        _controller = new(_keyVaultService, mapper);
    }

    [Test]
    public async Task GetAsync_Works_well()
    {
        // Arrange
        var keyVaultName = "KeyVaultName1";
        var tenantId = "tenantId1";
        var clientId = "clientId1";
        var clientSecret = "clientSecret1";
        var secretFilter = "SecretFilter";
        var request = GetRequest(keyVaultName, secretFilter, tenantId, clientId, clientSecret);

        var secretsEntity = GetSecretsEntity();
        var secretsGetResponse = GetSecretsGetResponse();

        _adapter.Setup(x => x.GetSecretsAsync(It.IsAny<CancellationToken>())).ReturnsAsync(secretsEntity);
        _adapter.Setup(x => x.Setup(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

        // Act
        var result = await _controller.GetAsync(request, default);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((SecretsGetResponse)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(secretsGetResponse);

        _adapter.Verify(x => x.Setup(keyVaultName, tenantId, clientId, clientSecret), Times.Once);
        _adapter.Verify(x => x.GetSecretsAsync(default), Times.Once);
    }

    [Test]
    public void GetDeleted_Works_well()
    {
        // Arrange
        var keyVaultName = "KeyVaultName1";
        var tenantId = "tenantId1";
        var clientId = "clientId1";
        var clientSecret = "clientSecret1";
        var secretFilter = "DeletedSecretFilter";
        var request = GetRequest(keyVaultName, secretFilter, tenantId, clientId, clientSecret);

        var secretsEntity = GetEmptyDeletedSecretsEntity();
        var secretsGetResponse = GetEmptySecretsGetResponse1();

        _adapter.Setup(x => x.GetDeletedSecrets(It.IsAny<CancellationToken>())).Returns(secretsEntity);
        _adapter.Setup(x => x.Setup(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

        // Act
        var result = _controller.GetDeleted(request, default);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((DeletedSecretsGetResponse)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(secretsGetResponse);

        _adapter.Verify(x => x.Setup(keyVaultName, tenantId, clientId, clientSecret), Times.Once);
        _adapter.Verify(x => x.GetDeletedSecrets(default), Times.Once);
    }

    [Test]
    public async Task DeleteAsync_Works_well()
    {
        // Arrange
        var keyVaultName = "KeyVaultName1";
        var tenantId = "tenantId1";
        var clientId = "clientId1";
        var clientSecret = "clientSecret1";
        var secretFilter = "SecretFilter";
        var request = GetRequest(keyVaultName, secretFilter, tenantId, clientId, clientSecret);

        var secretsEntity1 = GetSecretsEntity();
        var secretsEntity2 = GetEmptySecretsEntity();
        var secretsGetResponse = GetEmptySecretsGetResponse();

        _adapter.SetupSequence(x => x.GetSecretsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(secretsEntity1)
            .ReturnsAsync(secretsEntity2);

        _adapter.Setup(x => x.DeleteSecretAsync(It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(Status.Success);
        _adapter.Setup(x => x.Setup(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

        // Act
        var result = await _controller.DeleteAsync(request, default);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((SecretsGetResponse)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(secretsGetResponse);

        _adapter.Verify(x => x.Setup(keyVaultName, tenantId, clientId, clientSecret), Times.Once);
        _adapter.Verify(x => x.GetSecretsAsync(default), Times.Exactly(2));
        _adapter.Verify(x => x.DeleteSecretAsync(It.IsAny<string>(), default), Times.Exactly(3));
    }

    private static SecretRequest GetRequest(string keyVaultName, string secretFilter, string tenantId, string clientId, string clientSecret)
    {
        return new SecretRequest
        {
            KeyVaultName = keyVaultName,
            SecretFilter = secretFilter,
            TenantId = tenantId,
            ClientId = clientId,
            ClientSecret = clientSecret
        };
    }

    private static SecretsGetResponse GetSecretsGetResponse()
    {
        return new SecretsGetResponse
        {
            Status = Status.Success,
            Secrets = new List<SecretGetResponse>()
            {
                new()
                {
                    KeyVault = "KeyVaultName1",
                    SecretName = "SecretFilter123",
                    SecretValue = "3Kpu6gF214vAqHlzaX5G"
                },
                new()
                {
                    KeyVault = "KeyVaultName1",
                    SecretName = "SecretFilter456",
                    SecretValue = "KCRQJ08PdFHU9Ly2pUI2"
                },
                new()
                {
                    KeyVault = "KeyVaultName1",
                    SecretName = "SecretFilter789",
                    SecretValue = "ggl1oBLSiYNBliNQhsGW"
                }
            }
        };
    }

    private static SecretsEntity GetSecretsEntity()
    {
        return new SecretsEntity
        {
            Status = Status.Success,
            Secrets = new List<SecretEntity>()
            {
                new()
                {
                    Status = Status.Success,
                    Secret = new("SecretFilter123", "3Kpu6gF214vAqHlzaX5G")
                },
                new()
                {
                    Status = Status.Success,
                    Secret = new("SecretFilter456", "KCRQJ08PdFHU9Ly2pUI2")
                },
                new()
                {
                    Status = Status.Success,
                    Secret = new("SecretFilter789", "ggl1oBLSiYNBliNQhsGW")
                }
            }
        };
    }

    private static DeletedSecretsEntity GetEmptyDeletedSecretsEntity()
    {
        return new DeletedSecretsEntity
        {
            Status = Status.Success,
            DeletedSecrets = Enumerable.Empty<DeletedSecret>()
        };
    }

    private static SecretsEntity GetEmptySecretsEntity()
    {
        return new SecretsEntity
        {
            Status = Status.Success,
            Secrets = Enumerable.Empty<SecretEntity>()
        };
    }

    private static SecretsGetResponse GetEmptySecretsGetResponse()
    {
        return new SecretsGetResponse
        {
            Status = Status.Success,
            Secrets = Enumerable.Empty<SecretGetResponse>()

        };
    }

    private static DeletedSecretsGetResponse GetEmptySecretsGetResponse1()
    {
        return new DeletedSecretsGetResponse
        {
            Status = Status.Success,
            DeletedSecrets = Enumerable.Empty<DeletedSecretGetResponse>()
        };
    }
}
