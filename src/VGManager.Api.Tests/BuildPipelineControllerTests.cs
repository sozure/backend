using System.Text.Json;
using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using VGManager.Adapter.Client.Interfaces;
using VGManager.Adapter.Models.Models;
using VGManager.Adapter.Models.Response;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Api.Common;
using VGManager.Api.Handlers.Pipelines.BuildPipeline;
using VGManager.Services;
using VGManager.Services.Interfaces;

namespace VGManager.Api.Tests;

[TestFixture]
public class BuildPipelineControllerTests
{
    private Mock<IVGManagerAdapterClientService> _clientService;
    private IBuildPipelineService _buildPipelineService;

    [SetUp]
    public void Setup()
    {
        _clientService = new(MockBehavior.Strict);
        var adapterCommunicator = new AdapterCommunicator(_clientService.Object);
        _buildPipelineService = new BuildPipelineService(adapterCommunicator);
    }

    [Test]
    public async Task GetRepositoryIdAsync_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        var request = new BuildPipelineRequest
        {
            Organization = "MyOrganization",
            PAT = "q7Rt9p2X5yFvLmJhNzDcBwEaGtHxKvRq",
            Project = "VGManager.Library",
            DefinitionId = 1
        };

        var guid = Guid.NewGuid();

        var pipelineAdapterResponse = new BaseResponse<string>
        {
            Data = guid.ToString()
        };

        var repoAdapterResponse = new BaseResponse<IEnumerable<GitRepository>>
        {
            Data =
            [
                new()
                {
                    Id = guid,
                    Name = "VGManager.Library",
                }
            ]
        };

        var response = new AdapterResponseModel<Guid>
        {
            Data = guid,
            Status = AdapterStatus.Success
        };

        _clientService.Setup(x => x.SendAndReceiveMessageAsync("GetRepositoryIdByBuildPipelineRequest", It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, JsonSerializer.Serialize(pipelineAdapterResponse)));

        _clientService.Setup(x => x.SendAndReceiveMessageAsync("GetAllRepositoriesRequest", It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, JsonSerializer.Serialize(repoAdapterResponse)));

        // Act
        var result = await BuildPipelineHandler.GetRepositoryIdAsync(request, _buildPipelineService, default);

        // Assert
        result.Should().NotBeNull();

        _clientService.Verify(
            x => x.SendAndReceiveMessageAsync("GetRepositoryIdByBuildPipelineRequest", It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once
            );

        _clientService.Verify(
            x => x.SendAndReceiveMessageAsync("GetAllRepositoriesRequest", It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once
            );
    }

    [Test]
    public async Task GetAllAsync_Works_well()
    {
        // Arrange
        var request = new ExtendedBasicRequest
        {
            Organization = "MyOrganization",
            PAT = "q7Rt9p2X5yFvLmJhNzDcBwEaGtHxKvRq",
            Project = "VGManager.Library"
        };

        var pipelines = new List<Dictionary<string, string>>
        {
            new()
            {
                { "id", "1" },
                { "name", "VGManager.Library" }
            }
        };

        var adapterResponse = new BaseResponse<IEnumerable<BuildDefinitionReference>>
        {
            Data =
            [
                new()
                {
                    Id = 1,
                    Name = "VGManager.Library"
                }
            ]
        };

        var response = new AdapterResponseModel<IEnumerable<Dictionary<string, string>>>
        {
            Data = pipelines,
            Status = AdapterStatus.Success
        };

        _clientService.Setup(x => x.SendAndReceiveMessageAsync("GetBuildPipelinesRequest", It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, JsonSerializer.Serialize(adapterResponse)));

        // Act
        var result = await BuildPipelineHandler.GetAllAsync(request, _buildPipelineService, default);

        // Assert
        result.Should().NotBeNull();

        _clientService.Verify(
            x => x.SendAndReceiveMessageAsync("GetBuildPipelinesRequest", It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once
            );
    }

    [Test]
    public async Task RunPipelineAsync_Works_well()
    {
        // Arrange
        var request = new RunBuildRequest
        {
            Organization = "MyOrganization",
            PAT = "q7Rt9p2X5yFvLmJhNzDcBwEaGtHxKvRq",
            Project = "VGManager.Library",
            DefinitionId = 1,
            SourceBranch = "main"
        };

        var adapterResponse = new BaseResponse<AdapterStatus>
        {
            Data = AdapterStatus.Success
        };

        _clientService.Setup(x => x.SendAndReceiveMessageAsync("RunBuildPipelineRequest", It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, JsonSerializer.Serialize(adapterResponse)));

        // Act
        var result = await BuildPipelineHandler.RunBuildPipelineAsync(request, _buildPipelineService, default);

        // Assert
        result.Should().NotBeNull();

        _clientService.Verify(
            x => x.SendAndReceiveMessageAsync("RunBuildPipelineRequest", It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once
            );
    }

    [Test]
    public async Task RunBuildPipelinesAsync_Works_well()
    {
        // Arrange

        var request = new RunBuildPipelinesRequest
        {
            Organization = "MyOrganization",
            PAT = "q7Rt9p2X5yFvLmJhNzDcBwEaGtHxKvRq",
            Project = "VGManager.Library",
            Pipelines = new List<Dictionary<string, string>>
            {
                new()
                {
                    { "id", "1" },
                    { "name", "VGManager.Library" }
                }
            }
        };

        var adapterResponse = new BaseResponse<AdapterStatus>
        {
            Data = AdapterStatus.Success
        };

        _clientService.Setup(x => x.SendAndReceiveMessageAsync("RunBuildPipelinesRequest", It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, JsonSerializer.Serialize(adapterResponse)));

        // Act
        var result = await BuildPipelineHandler.RunBuildPipelinesAsync(request, _buildPipelineService, default);

        // Assert
        result.Should().NotBeNull();
    }
}
