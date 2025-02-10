using System.Text.Json;
using AutoMapper;
using VGManager.Adapter.Client.Interfaces;
using VGManager.Adapter.Models.Models;
using VGManager.Adapter.Models.Response;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Api.Common;
using VGManager.Api.Handlers.GitVersion;
using VGManager.Api.MapperProfiles;
using VGManager.Services;
using VGManager.Services.Helper;
using VGManager.Services.Interfaces;
using VGManager.Services.Models;

namespace VGManager.Api.Tests;

[TestFixture]
public class GitVersionControllerTests
{
    private IGitVersionService _gitVersionService;
    private IMapper _mapper;
    private Mock<IVGManagerAdapterClientService> _clientService = null!;

    [SetUp]
    public void Setup()
    {
        _clientService = new(MockBehavior.Strict);

        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(GitVersionProfile));
        });

        _mapper = mapperConfiguration.CreateMapper();

        var adapterCommunicator = new AdapterCommunicator(_clientService.Object);
        var gitAdapterCommunicatorService = new GitAdapterCommunicatorService(adapterCommunicator);
        _gitVersionService = new GitVersionService(gitAdapterCommunicatorService, adapterCommunicator);
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
        var result = await GitVersionHandler.GetBranchesAsync(request, _gitVersionService, default);

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
    public async Task GetBranchesAsync_Throws_exception()
    {
        // Arrange
        var request = new GitBasicRequest
        {
            Organization = "MyOrganization",
            PAT = "q7Rt9p2X5yFvLmJhNzDcBwEaGtHxKvRq",
            RepositoryId = "1234567"
        };

        var response = new AdapterResponseModel<IEnumerable<string>>
        {
            Data = [],
            Status = AdapterStatus.Unknown
        };

        _clientService.Setup(x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());

        // Act
        var result = await GitVersionHandler.GetBranchesAsync(request, _gitVersionService, default);

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
        var result = await GitVersionHandler.GetTagsAsync(request, _gitVersionService, default);

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
    public async Task GetTagsAsync_Throws_exception()
    {
        // Arrange
        var request = new GitBasicRequest
        {
            Organization = "MyOrganization",
            PAT = "q7Rt9p2X5yFvLmJhNzDcBwEaGtHxKvRq",
            RepositoryId = "f47ac10b-58cc-4372-a567-0e02b2c3d479"
        };

        var response = new AdapterResponseModel<IEnumerable<string>>
        {
            Data = [],
            Status = AdapterStatus.Unknown
        };

        _clientService.Setup(x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());

        // Act
        var result = await GitVersionHandler.GetTagsAsync(request, _gitVersionService, default);

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
        var result = await GitVersionHandler.CreateTagAsync(request, _gitVersionService, default);

        // Assert
        result.Should().NotBeNull();
        //result.Result.Should().BeOfType<OkObjectResult>();
        //((AdapterResponseModel<string>)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(response);

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

    [Test]
    public async Task CreateTagAsync_Throws_exception()
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

        var response = new AdapterResponseModel<string>
        {
            Data = string.Empty,
            Status = AdapterStatus.Unknown
        };

        _clientService.Setup(x => x.SendAndReceiveMessageAsync("GetBranchesRequest", It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException());

        // Act
        var result = await GitVersionHandler.CreateTagAsync(request, _gitVersionService, default);

        // Assert
        result.Should().NotBeNull();
        //result.Result.Should().BeOfType<OkObjectResult>();
        //((AdapterResponseModel<string>)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(response);

        _clientService.Verify(
            x => x.SendAndReceiveMessageAsync("GetTagsRequest", It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never
            );

        _clientService.Verify(
            x => x.SendAndReceiveMessageAsync("GetBranchesRequest", It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once
            );

        _clientService.Verify(
            x => x.SendAndReceiveMessageAsync("CreateTagRequest", It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Never
            );
    }
}
