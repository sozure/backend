using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Services.Profile;
using System.Text.Json;
using VGManager.Adapter.Client.Interfaces;
using VGManager.Adapter.Models.Models;
using VGManager.Adapter.Models.Response;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Api.Common;
using VGManager.Api.Endpoints.UserProfile;
using VGManager.Services;

namespace VGManager.Api.Tests;

[TestFixture]
public class ProfileControllerTests
{
    private ProfileController _profileController;

    private Mock<IVGManagerAdapterClientService> _clientService = null!;

    [SetUp]
    public void Setup()
    {
        _clientService = new(MockBehavior.Strict);
        var adapterCommunicator = new AdapterCommunicator(_clientService.Object);
        var profileService = new ProfileService(adapterCommunicator);
        _profileController = new ProfileController(profileService);
    }

    [Test]
    public async Task GetAsync_Works_well()
    {
        // Arrange
        var request = new BasicRequest
        {
            Organization = "MyOrganization",
            PAT = "q7Rt9p2X5yFvLmJhNzDcBwEaGtHxKvRq",
        };

        var adapterResponse = new BaseResponse<Profile?>
        {
            Data = new Profile
            {
                DisplayName = "John Doe",
            }
        };

        _clientService.Setup(x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, JsonSerializer.Serialize(adapterResponse)));

        // Act
        var result = await _profileController.GetAsync(request, default);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((AdapterResponseModel<Profile>)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(adapterResponse);

        _clientService.Verify(x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task GetAsync_Returns_null()
    {
        // Arrange
        var request = new BasicRequest
        {
            Organization = "MyOrganization",
            PAT = "q7Rt9p2X5yFvLmJhNzDcBwEaGtHxKvRq",
        };

        var adapterResponse = new BaseResponse<Profile?>
        {
            Data = null!,
        };

        var response = new AdapterResponseModel<Profile>()
        {
            Data = null!,
            Status = AdapterStatus.Unknown
        };

        _clientService.Setup(x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((true, JsonSerializer.Serialize(adapterResponse)));

        // Act
        var result = await _profileController.GetAsync(request, default);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((AdapterResponseModel<Profile>)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(response);

        _clientService.Verify(x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Test]
    public async Task GetAsync_Throws_error()
    {
        // Arrange
        var request = new BasicRequest
        {
            Organization = "MyOrganization",
            PAT = "q7Rt9p2X5yFvLmJhNzDcBwEaGtHxKvRq",
        };

        var response = new AdapterResponseModel<Profile>()
        {
            Data = null!,
            Status = AdapterStatus.Unknown
        };

        _clientService.Setup(x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidDataException());

        // Act
        var result = await _profileController.GetAsync(request, default);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((AdapterResponseModel<Profile>)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(response);

        _clientService.Verify(x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
