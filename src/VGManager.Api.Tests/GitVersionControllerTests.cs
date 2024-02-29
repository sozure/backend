using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using VGManager.Adapter.Client.Interfaces;
using VGManager.Adapter.Models.Models;
using VGManager.Adapter.Models.Requests;
using VGManager.Adapter.Models.Response;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Api.Common;
using VGManager.Api.Endpoints.GitBranch;
using VGManager.Services;
using VGManager.Services.Models;

namespace VGManager.Api.Tests;

[TestFixture]
public class GitVersionControllerTests
{
    private GitVersionController _gitVersionController;
    private Mock<IVGManagerAdapterClientService> _clientService = null!;

    [SetUp]
    public void Setup()
    {
        _clientService = new(MockBehavior.Strict);
        var adapterCommunicator = new AdapterCommunicator(_clientService.Object);
        var gitVersionService = new GitVersionService(adapterCommunicator);
        _gitVersionController = new GitVersionController(gitVersionService);
    }

    [Test]
    public async Task GetBranchesAsync_Works_well()
    {
        // Arrange
        var request = new GitBasicRequest
        {
            Organization = "MyOrganization",
            PAT = "q7Rt9p2X5yFvLmJhNzDcBwEaGtHxKvRq",
            RepositoryId = "1234567"
        };

        var branches = new List<string> { "branch1", "branch2" };

        var adapterResponse = new BaseResponse<Dictionary<string, object>>
        {
            Data = new Dictionary<string, object>
            {
                { "Status", "1" },
                { "Data", branches }
            }
        };

        var response = new AdapterResponseModel<IEnumerable<string>>
        {
            Data = branches,
            Status = AdapterStatus.Success
        };

        _clientService.Setup(x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, JsonSerializer.Serialize(adapterResponse)));

        // Act
        var result = await _gitVersionController.GetBranchesAsync(request, default);

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
    public async Task GetTagsAsync_Works_well()
    {
        // Arrange
        var request = new GitBasicRequest
        {
            Organization = "MyOrganization",
            PAT = "q7Rt9p2X5yFvLmJhNzDcBwEaGtHxKvRq",
            RepositoryId = "f47ac10b-58cc-4372-a567-0e02b2c3d479"
        };

        var tags = new List<string> { "refs/tags/1.0.0", "refs/tags/1.1.0" };

        var adapterResponse = new BaseResponse<Dictionary<string, object>>
        {
            Data = new Dictionary<string, object>
            {
                { "Status", "1" },
                { "Data", tags }
            }
        };

        var response = new AdapterResponseModel<IEnumerable<string>>
        {
            Data = tags,
            Status = AdapterStatus.Success
        };

        _clientService.Setup(x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, JsonSerializer.Serialize(adapterResponse)));

        // Act
        var result = await _gitVersionController.GetTagsAsync(request, default);

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
    public async Task CreateTagAsync_Works_well()
    {
        // Arrange
        var request = new CreateTagEntity
        {
            Organization = "MyOrganization",
            PAT = "q7Rt9p2X5yFvLmJhNzDcBwEaGtHxKvRq",
            RepositoryId = new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
            TagName = "major",
            Description = "This is a new tag",
            Project = "VGManager.Library",
            UserName = "JohnDoe",
        };

        var tags = new List<string> { "refs/tags/1.0.0", "refs/tags/1.1.0" };
        var branches = new List<string> { "main", "develop" };
        var newTag = "1.2.0";

        var tagsAdapterResponse = new BaseResponse<Dictionary<string, object>>
        {
            Data = new Dictionary<string, object>
            {
                { "Status", "1" },
                { "Data", tags }
            }
        };

        var branchesAdapterResponse = new BaseResponse<Dictionary<string, object>>
        {
            Data = new Dictionary<string, object>
            {
                { "Status", "1" },
                { "Data", branches }
            }
        };

        var newTagAdapterResponse = new BaseResponse<Dictionary<string, object>>
        {
            Data = new Dictionary<string, object>
            {
                { "Status", "1" },
                { "Data", newTag }
            }
        };

        var response = new AdapterResponseModel<string>
        {
            Data = newTag,
            Status = AdapterStatus.Success
        };

        _clientService.Setup(x => x.SendAndReceiveMessageAsync("GetTagsRequest", It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, JsonSerializer.Serialize(tagsAdapterResponse)));

        _clientService.Setup(x => x.SendAndReceiveMessageAsync("GetBranchesRequest", It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, JsonSerializer.Serialize(branchesAdapterResponse)));

        _clientService.Setup(x => x.SendAndReceiveMessageAsync("CreateTagRequest", It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, JsonSerializer.Serialize(newTagAdapterResponse)));

        // Act
        var result = await _gitVersionController.CreateTagAsync(request, default);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((AdapterResponseModel<string>)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(response);

        _clientService.Verify(
            x => x.SendAndReceiveMessageAsync("GetTagsRequest", It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once
            );

        _clientService.Verify(
            x => x.SendAndReceiveMessageAsync("GetBranchesRequest", It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once
            );

        _clientService.Verify(
            x => x.SendAndReceiveMessageAsync("CreateTagRequest", It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once
            );
    }
}