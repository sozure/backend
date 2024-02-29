using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using VGManager.Adapter.Client.Interfaces;
using VGManager.Adapter.Models.Models;
using VGManager.Adapter.Models.Response;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Api.Endpoints.GitRepository;
using VGManager.Api.Endpoints.GitRepository.Request;
using VGManager.Api.MapperProfiles;
using VGManager.Services;
using VGManager.Services.Models.GitRepositories;

namespace VGManager.Api.Tests;

[TestFixture]
public class GitRepositoryControllerTests
{
    private GitRepositoryController _gitRepositoryController;
    private Mock<IVGManagerAdapterClientService> _clientService = null!;

    [SetUp]
    public void Setup()
    {
        _clientService = new(MockBehavior.Strict);

        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(GitRepositoryProfile));
        });

        var mapper = mapperConfiguration.CreateMapper();

        var adapterCommunicator = new AdapterCommunicator(_clientService.Object);
        var gitRepositoryService = new GitRepositoryService(adapterCommunicator);
        _gitRepositoryController = new GitRepositoryController(gitRepositoryService, mapper);
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
                    Name = "VGManager.Library"
                }
        };

        var repositoriesRes = new List<GitRepositoryResult>
        {
            new()
                {
                    RepositoryId = repositories.FirstOrDefault()?.Id.ToString() ?? string.Empty,
                    RepositoryName = repositories.FirstOrDefault()?.Name ?? string.Empty
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
        var result = await _gitRepositoryController.GetAsync(request, new CancellationToken());

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((AdapterResponseModel<IEnumerable<GitRepositoryResult>>)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(response);

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
            Exceptions = Enumerable.Empty<string>(),
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
        var result = await _gitRepositoryController.GetVariablesAsync(request, default);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((AdapterResponseModel<IEnumerable<string>>)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(response);

        _clientService.Verify(
            x => x.SendAndReceiveMessageAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()),
            Times.Once
            );
    }
}
