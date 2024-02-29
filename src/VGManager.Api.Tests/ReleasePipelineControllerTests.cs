using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Text.Json;
using VGManager.Adapter.Client.Interfaces;
using VGManager.Adapter.Models.Models;
using VGManager.Adapter.Models.Response;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Api.Endpoints.Pipelines.Release;
using VGManager.Api.Endpoints.Pipelines.Release.Request;
using VGManager.Services;

namespace VGManager.Api.Tests;

[TestFixture]
public class ReleasePipelineControllerTests
{
    private ReleasePipelineController _controller;
    private Mock<IVGManagerAdapterClientService> _clientService;

    [SetUp]
    public void Setup()
    {
        _clientService = new(MockBehavior.Strict);
        var loggerMock = new Mock<ILogger<ReleasePipelineService>>();

        var adapterCommunicator = new AdapterCommunicator(_clientService.Object);
        var service = new ReleasePipelineService(adapterCommunicator, loggerMock.Object);
        _controller = new ReleasePipelineController(service);
    }

    [Test]
    public async Task GetEnvironmentsAsync_Works_well()
    {
        // Arrange
        var request = new ReleasePipelineRequest
        {
            Organization = "MyOrganization",
            PAT = "q7Rt9p2X5yFvLmJhNzDcBwEaGtHxKvRq",
            Project = "beviktor95",
            RepositoryName = "VGManager.Library",
            ConfigFile = "appsettings.Development.json"
        };

        var environments = new List<string> { "env1", "env2" };

        var adapterResponse = new BaseResponse<Dictionary<string, object>>
        {
            Data = new Dictionary<string, object>
            {
                { "Status", "1" },
                { "Data", environments }
            }
        };

        var response = new AdapterResponseModel<IEnumerable<string>>
        {
            Data = environments,
            Status = AdapterStatus.Success
        };

        _clientService.Setup(x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, JsonSerializer.Serialize(adapterResponse)));

        // Act
        var result = await _controller.GetEnvironmentsAsync(request, default);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((AdapterResponseModel<IEnumerable<string>>)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(response);

        _clientService.Verify(
            x => x.SendAndReceiveMessageAsync("GetEnvironmentsRequest", It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once
            );
    }

    [Test]
    public async Task GetVariableGroupsAsync_Works_well()
    {
        // Arrange
        var request = new ReleasePipelineRequest
        {
            Organization = "MyOrganization",
            PAT = "q7Rt9p2X5yFvLmJhNzDcBwEaGtHxKvRq",
            Project = "beviktor95",
            RepositoryName = "VGManager.Library",
            ConfigFile = "appsettings.Development.json"
        };

        var adapterResponse = new BaseResponse<Dictionary<string, object>>
        {
            Data = new Dictionary<string, object>
            {
                { "Status", "1" },
                { "Data", new List<Dictionary<string, string>> { GetSampleDictionary() } }
            }
        };

        var response = new AdapterResponseModel<IEnumerable<Dictionary<string, string>>>
        {
            Data = new List<Dictionary<string, string>>
            { 
                GetSampleDictionary()
            },
            Status = AdapterStatus.Success
        };

        _clientService.Setup(x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, JsonSerializer.Serialize(adapterResponse)));

        // Act
        var result = await _controller.GetVariableGroupsAsync(request, default);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((AdapterResponseModel<IEnumerable<Dictionary<string, string>>>)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(response);

        _clientService.Verify(
            x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once
            );
    }

    [Test]
    public async Task GetProjectsWithCorrespondingReleasePipelineAsync_Works_well()
    {
        // Arrange
        var projects = new List<string> { "beviktor95" };
        var request = new ProjectsWithCorrespondingReleasePipelineRequest
        {
            Organization = "MyOrganization",
            PAT = "q7Rt9p2X5yFvLmJhNzDcBwEaGtHxKvRq",
            RepositoryName = "VGManager.Library",
            ConfigFile = "appsettings.Development.json",
            Projects = projects
        };

        var environments = new List<string> { "env1", "env2" };

        var adapterResponse = new BaseResponse<Dictionary<string, object>>
        {
            Data = new Dictionary<string, object>
            {
                { "Status", "1" },
                { "Data", environments }
            }
        };

        var response = new AdapterResponseModel<IEnumerable<string>>
        {
            Data = projects,
            Status = AdapterStatus.Success
        };

        _clientService.Setup(x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, JsonSerializer.Serialize(adapterResponse)));

        // Act
        var result = await _controller.GetProjectsWithCorrespondingReleasePipelineAsync(request, default);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((AdapterResponseModel<IEnumerable<string>>)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(response);

        _clientService.Verify(
            x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once
            );
    }

    private static Dictionary<string, string> GetSampleDictionary()
    {
        return new()
                {
                    { "Name", "VGManager.Libary Staging Secret" },
                    { "Type", "AzureKeyVault" }
                };
    }
}
