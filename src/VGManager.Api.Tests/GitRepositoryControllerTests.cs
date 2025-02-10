using System.Text.Json;
using AutoMapper;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using VGManager.Adapter.Client.Interfaces;
using VGManager.Adapter.Models.Models;
using VGManager.Adapter.Models.Response;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Api.Handlers.GitRepository;
using VGManager.Api.Handlers.GitRepository.Request;
using VGManager.Api.MapperProfiles;
using VGManager.Services;
using VGManager.Services.Interfaces;
using VGManager.Services.Models.GitRepositories;

namespace VGManager.Api.Tests;

[TestFixture]
public class GitRepositoryControllerTests
{
    private IGitRepositoryService _gitRepositoryService;
    private IMapper _mapper;
    private Mock<IVGManagerAdapterClientService> _clientService = null!;

    [SetUp]
    public void Setup()
    {
        _clientService = new(MockBehavior.Strict);

        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(GitRepositoryProfile));
        });

        _mapper = mapperConfiguration.CreateMapper();

        var adapterCommunicator = new AdapterCommunicator(_clientService.Object);
        _gitRepositoryService = new GitRepositoryService(adapterCommunicator);
    }

    [Test]
    public async Task GetAsync_Works_well()
    {
        // Arrange
        var request = new GitRepositoryBaseRequest
        {
            Organization = "MyOrganization",
            PAT = "q7Rt9p2X5yFvLmJhNzDcBwEaGtHxKvRq",
            Project = "beviktor95",
        };

        var repositories = new List<GitRepository>
        {
            new()
                {
                    Id = Guid.NewGuid(),
                    Name = "VGManager.Library",
                    ProjectReference = new TeamProjectReference
                    {
                        Name = request.Project
                    }
                }
        };

        var repositoriesRes = new List<GitRepositoryResult>
        {
            new()
                {
                    RepositoryId = repositories.FirstOrDefault()?.Id.ToString() ?? string.Empty,
                    RepositoryName = repositories.FirstOrDefault()?.Name ?? string.Empty,
                    ProjectName = repositories.FirstOrDefault()?.ProjectReference.Name ?? string.Empty
                }
        };

        var response = new AdapterResponseModel<IEnumerable<GitRepositoryResult>>
        {
            Data = repositoriesRes,
            Status = AdapterStatus.Success
        };

        var adapterResponse = new BaseResponse<IEnumerable<GitRepository>>
        {
            Data = repositories
        };

        _clientService.Setup(x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, JsonSerializer.Serialize(adapterResponse)));

        // Act
        var result = await GitRepositoryHandler.GetAsync(request, _gitRepositoryService, default);

        // Assert
        result.Should().NotBeNull();
        //result.Result.Should().BeOfType<OkObjectResult>();
        //((AdapterResponseModel<IEnumerable<GitRepositoryResult>>)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(response);

        _clientService.Verify(
            x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once
            );
    }

    [Test]
    public async Task GetAsync_Adapter_returns_success_false()
    {
        // Arrange
        var request = new GitRepositoryBaseRequest
        {
            Organization = "MyOrganization",
            PAT = "q7Rt9p2X5yFvLmJhNzDcBwEaGtHxKvRq",
            Project = "beviktor95",
        };

        var response = new AdapterResponseModel<IEnumerable<GitRepositoryResult>>
        {
            Data = [],
            Status = AdapterStatus.Unknown
        };

        _clientService.Setup(x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((false, JsonSerializer.Serialize((BaseResponse<IEnumerable<GitRepository>>)null!)));

        // Act
        var result = await GitRepositoryHandler.GetAsync(request, _gitRepositoryService, default);

        // Assert
        result.Should().NotBeNull();
        //result.Result.Should().BeOfType<OkObjectResult>();
        //((AdapterResponseModel<IEnumerable<GitRepositoryResult>>)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(response);

        _clientService.Verify(
            x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once
            );
    }

    [Test]
    public async Task GetAsync_Adapter_returns_null()
    {
        // Arrange
        var request = new GitRepositoryBaseRequest
        {
            Organization = "MyOrganization",
            PAT = "q7Rt9p2X5yFvLmJhNzDcBwEaGtHxKvRq",
            Project = "beviktor95",
        };

        var response = new AdapterResponseModel<IEnumerable<GitRepositoryResult>>
        {
            Data = [],
            Status = AdapterStatus.Unknown
        };

        _clientService.Setup(x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, JsonSerializer.Serialize((BaseResponse<IEnumerable<GitRepository>>)null!)));

        // Act
        var result = await GitRepositoryHandler.GetAsync(request, _gitRepositoryService, default);

        // Assert
        result.Should().NotBeNull();
        //result.Result.Should().BeOfType<OkObjectResult>();
        //((AdapterResponseModel<IEnumerable<GitRepositoryResult>>)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(response);

        _clientService.Verify(
            x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once
            );
    }

    [Test]
    public async Task GetVariablesAsync_Works_well()
    {
        // Arrange
        var request = new GitRepositoryVariablesRequest
        {
            Organization = "MyOrganization",
            PAT = "q7Rt9p2X5yFvLmJhNzDcBwEaGtHxKvRq",
            RepositoryId = "VGManager.Library",
            Branch = "main",
            Delimiter = "__",
            Exceptions = [],
            FilePath = ".src/VGManager.Library.Api/appsettings.Development.json",
            Project = "beviktor95"
        };

        var variables = new List<string>
        {
            "VGManager.Library.Api:appsettings.Development.json:ConnectionStrings:DefaultConnection",
            "VGManager.Library.Api:appsettings.Development.json:Logging:LogLevel:Default"
        };

        var adapterResponse = new BaseResponse<List<string>>
        {
            Data = variables,
        };

        var response = new AdapterResponseModel<IEnumerable<string>>
        {
            Data = variables,
            Status = AdapterStatus.Success
        };

        _clientService.Setup(x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, JsonSerializer.Serialize(adapterResponse)));

        // Act
        var result = await GitRepositoryHandler.GetVariablesAsync(request, _gitRepositoryService, _mapper, default);

        // Assert
        result.Should().NotBeNull();
        //result.Result.Should().BeOfType<OkObjectResult>();
        //((AdapterResponseModel<IEnumerable<string>>)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(response);

        _clientService.Verify(
            x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once
            );
    }

    [Test]
    public async Task GetVariablesAsync_Adapter_returns_success_false()
    {
        // Arrange
        var request = new GitRepositoryVariablesRequest
        {
            Organization = "MyOrganization",
            PAT = "q7Rt9p2X5yFvLmJhNzDcBwEaGtHxKvRq",
            RepositoryId = "VGManager.Library",
            Branch = "main",
            Delimiter = "__",
            Exceptions = [],
            FilePath = ".src/VGManager.Library.Api/appsettings.Development.json",
            Project = "beviktor95"
        };

        var response = new AdapterResponseModel<IEnumerable<string>>
        {
            Data = [],
            Status = AdapterStatus.Unknown
        };

        _clientService.Setup(x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((false, JsonSerializer.Serialize((BaseResponse<List<string>>)null!)));

        // Act
        var result = await GitRepositoryHandler.GetVariablesAsync(request, _gitRepositoryService, _mapper, default);

        // Assert
        result.Should().NotBeNull();
        //result.Result.Should().BeOfType<OkObjectResult>();
        //((AdapterResponseModel<IEnumerable<string>>)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(response);

        _clientService.Verify(
            x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once
            );
    }

    [Test]
    public async Task GetVariablesAsync_Adapter_returns_null()
    {
        // Arrange
        var request = new GitRepositoryVariablesRequest
        {
            Organization = "MyOrganization",
            PAT = "q7Rt9p2X5yFvLmJhNzDcBwEaGtHxKvRq",
            RepositoryId = "VGManager.Library",
            Branch = "main",
            Delimiter = "__",
            Exceptions = [],
            FilePath = ".src/VGManager.Library.Api/appsettings.Development.json",
            Project = "beviktor95"
        };

        var response = new AdapterResponseModel<IEnumerable<string>>
        {
            Data = [],
            Status = AdapterStatus.Unknown
        };

        _clientService.Setup(x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, JsonSerializer.Serialize((BaseResponse<List<string>>)null!)));

        // Act
        var result = await GitRepositoryHandler.GetVariablesAsync(request, _gitRepositoryService, _mapper, default);

        // Assert
        result.Should().NotBeNull();
        //result.Result.Should().BeOfType<OkObjectResult>();
        //((AdapterResponseModel<IEnumerable<string>>)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(response);

        _clientService.Verify(
            x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once
            );
    }
}
