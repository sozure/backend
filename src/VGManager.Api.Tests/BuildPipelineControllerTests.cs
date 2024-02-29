using Microsoft.AspNetCore.Mvc;
using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using System.Text.Json;
using VGManager.Adapter.Client.Interfaces;
using VGManager.Adapter.Models.Models;
using VGManager.Adapter.Models.Response;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Api.Common;
using VGManager.Api.Endpoints.Pipelines.Build;
using VGManager.Api.Endpoints.Pipelines.BuildPipeline;
using VGManager.Services;

namespace VGManager.Api.Tests;

[TestFixture]
public class BuildPipelineControllerTests
{
    private BuildPipelineController _controller;
    private Mock<IVGManagerAdapterClientService> _clientService;

    [SetUp]
    public void Setup()
    {
        _clientService = new(MockBehavior.Strict);
        var adapterCommunicator = new AdapterCommunicator(_clientService.Object);
        var service = new BuildPipelineService(adapterCommunicator);
        _controller = new BuildPipelineController(service);
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

        var pipelineAdapterResponse = new BaseResponse<BuildDefinitionReference>
        {
            Data = new BuildDefinitionReference
            {
                Id = 1,
                Name = "VGManager.Library"
            }
        };

        var repoAdapterResponse = new BaseResponse<IEnumerable<GitRepository>>
        {
            Data = new List<GitRepository>
            {
                new()
                {
                    Id = guid,
                    Name = "VGManager.Library"
                }
            }
        };

        var response = new AdapterResponseModel<Guid>
        {
            Data = guid,
            Status = AdapterStatus.Success
        };

        _clientService.Setup(x => x.SendAndReceiveMessageAsync("GetBuildPipelineRequest", It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, JsonSerializer.Serialize(pipelineAdapterResponse)));

        _clientService.Setup(x => x.SendAndReceiveMessageAsync("GetAllRepositoriesRequest", It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, JsonSerializer.Serialize(repoAdapterResponse)));

        // Act
        var result = await _controller.GetRepositoryIdAsync(request, default);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((AdapterResponseModel<Guid>)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(response);

        _clientService.Verify(
            x => x.SendAndReceiveMessageAsync("GetBuildPipelineRequest", It.IsAny<string>(), It.IsAny<CancellationToken>()),
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
            Data = new List<BuildDefinitionReference>
            {
                new()
                {
                    Id = 1,
                    Name = "VGManager.Library"
                }
            }
        };

        var response = new AdapterResponseModel<IEnumerable<Dictionary<string, string>>>
        {
            Data = pipelines,
            Status = AdapterStatus.Success
        };

        _clientService.Setup(x => x.SendAndReceiveMessageAsync("GetBuildPipelinesRequest", It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, JsonSerializer.Serialize(adapterResponse)));

        // Act
        var result = await _controller.GetAllAsync(request, default);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((AdapterResponseModel<IEnumerable<Dictionary<string, string>>>)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(response);

        _clientService.Verify(
            x => x.SendAndReceiveMessageAsync("GetBuildPipelinesRequest", It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once
            );
    }

    [Test]
    public async Task RunPipelineAsync_Works_well()
    {
        // Arrange
        var request = new RunBuildPipelineRequest
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
        var result = await _controller.RunBuildPipelineAsync(request, default);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((AdapterStatus)((OkObjectResult)result.Result!).Value!).Should().Be(AdapterStatus.Success);

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
        var result = await _controller.RunBuildPipelinesAsync(request, default);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((AdapterStatus)((OkObjectResult)result.Result!).Value!).Should().Be(AdapterStatus.Success);
    }
}
