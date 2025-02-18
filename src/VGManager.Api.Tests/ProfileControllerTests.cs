using System.Text.Json;
using Microsoft.VisualStudio.Services.Profile;
using VGManager.Adapter.Client.Interfaces;
using VGManager.Adapter.Models.Models;
using VGManager.Adapter.Models.Response;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Api.Common;
using VGManager.Api.Handlers.Profile;
using VGManager.Services;
using VGManager.Services.Interfaces;

namespace VGManager.Api.Tests;

[TestFixture]
public class ProfileControllerTests
{
    private Mock<IVGManagerAdapterClientService> _clientService = null!;
    private IProfileService _profileService;

    [SetUp]
    public void Setup()
    {
        _clientService = new(MockBehavior.Strict);
        var adapterCommunicator = new AdapterCommunicator(_clientService.Object);
        _profileService = new ProfileService(adapterCommunicator);
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
        var result = await ProfileHandler.GetProfileAsync(request, _profileService, default);

        // Assert
        result.Should().NotBeNull();

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
        var result = await ProfileHandler.GetProfileAsync(request, _profileService, default);

        // Assert
        result.Should().NotBeNull();

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
        var result = await ProfileHandler.GetProfileAsync(request, _profileService, default);

        // Assert
        result.Should().NotBeNull();

        _clientService.Verify(x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
