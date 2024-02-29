using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using VGManager.Adapter.Client.Interfaces;
using VGManager.Adapter.Models.Models;
using VGManager.Adapter.Models.Response;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Api.Endpoints.GitFile;
using VGManager.Services;
using VGManager.Services.Helper;

namespace VGManager.Api.Tests;

[TestFixture]
public class GitFileControllerTests
{
    private GitFileController _gitFileController;
    private Mock<IVGManagerAdapterClientService> _clientService;

    [SetUp]
    public void Setup()
    {
        _clientService = new(MockBehavior.Strict);
        var adapterCommunicatorService = new AdapterCommunicator(_clientService.Object);
        var gitAdapterCommunicatorService = new GitAdapterCommunicatorService(adapterCommunicatorService);
        var gitFileService = new GitFileService(gitAdapterCommunicatorService);
        _gitFileController = new GitFileController(gitFileService);
    }

    [Test]
    public async Task GetFilePathAsync_ReturnsOk()
    {
        // Arrange
        var request = new GitFilePathRequest
        {
            Organization = "MyOrganization",
            PAT = "q7Rt9p2X5yFvLmJhNzDcBwEaGtHxKvRq",
            RepositoryId = "VGManager.Library",
            FileName = "appsettings.Development.json",
            Branch = "main",
        };

        var path = new List<string>
        {
            ".src/VGManager.Library.Api/appsettings.Development.json"
        };

        var response = new AdapterResponseModel<IEnumerable<string>>
        {
            Data = path,
            Status = AdapterStatus.Success
        };

        var adapterResponse = new BaseResponse<Dictionary<string, object>>
        {
            Data = new Dictionary<string, object>
            {
                { "Status", "1" },
                { "Data",  path }
            }
        };

        _clientService.Setup(x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, JsonSerializer.Serialize(adapterResponse)));

        // Act
        var result = await _gitFileController.GetFilePathAsync(request, default);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((AdapterResponseModel<IEnumerable<string>>)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(response);

        _clientService.Verify(
            x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once
            );
    }

    [Test]
    public async Task GetConfigFilesAsync_Works_well()
    {
        // Arrange
        var request = new GitConfigFileRequest
        {
            Organization = "MyOrganization",
            PAT = "q7Rt9p2X5yFvLmJhNzDcBwEaGtHxKvRq",
            RepositoryId = "VGManager.Library",
            Extension = "json",
            Branch = "main",
        };

        var path = new List<string>
        {
            ".src/VGManager.Library.Api/appsettings.Development.json"
        };

        var response = new AdapterResponseModel<IEnumerable<string>>
        {
            Data = path,
            Status = AdapterStatus.Success
        };

        var adapterResponse = new BaseResponse<Dictionary<string, object>>
        {
            Data = new Dictionary<string, object>
            {
                { "Status", "1" },
                { "Data",  path }
            }
        };

        _clientService.Setup(x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, JsonSerializer.Serialize(adapterResponse)));

        // Act
        var result = await _gitFileController.GetConfigFilesAsync(request, default);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((AdapterResponseModel<IEnumerable<string>>)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(response);
    }
}
