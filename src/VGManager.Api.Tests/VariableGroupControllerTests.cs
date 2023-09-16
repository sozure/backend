using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.DistributedTask.WebApi;
using VGManager.Api.Controllers;
using VGManager.Api.MapperProfiles;
using VGManager.Api.VariableGroups.Response;
using VGManager.AzureAdapter.Entities;
using VGManager.AzureAdapter.Interfaces;
using VGManager.Services;
using ProjectProfile = VGManager.Services.MapperProfiles.ProjectProfile;

namespace VGManager.Api.Tests;

[TestFixture]
public class VariableGroupControllerTests
{
    private VariableGroupController _controller;
    private Mock<IVariableGroupAdapter> _variableGroupAdapter;
    private Mock<IProjectAdapter> _projectAdapter;

    [SetUp]
    public void Setup()
    {
        var mapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(VariableGroupProfile));
        });

        var serviceMapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(ProjectProfile));
        });

        var mapper = mapperConfiguration.CreateMapper();
        var serviceMapper = serviceMapperConfiguration.CreateMapper();

        _variableGroupAdapter = new(MockBehavior.Strict);
        _projectAdapter = new(MockBehavior.Strict);
        var loggerMock = new Mock<ILogger<VariableGroupService>>();

        var vgService = new VariableGroupService(_variableGroupAdapter.Object, loggerMock.Object);
        var projectService = new ProjectService(_projectAdapter.Object, serviceMapper);

        _controller = new(vgService, projectService, mapper);
    }

    [Test]
    public async Task GetAsync_Works_well_1()
    {
        // Arrange
        var organization = "Organization1";
        var pat = "WtxMFit1uz1k64u527mB";
        var project = "Project1";
        var valueFilter = "value";

        var variableRequest = TestSampleData.GetVariableRequest(organization, pat, project, "key", valueFilter);

        var variableGroupEntity = TestSampleData.GetVariableGroupEntity();
        var variableGroupResponse = TestSampleData.GetVariableGroupGetResponses();

        _variableGroupAdapter.Setup(x => x.Setup(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        _variableGroupAdapter.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(variableGroupEntity);

        // Act
        var result = await _controller.GetAsync(variableRequest, default);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((VariableGroupResponses)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(variableGroupResponse);

        _variableGroupAdapter.Verify(x => x.GetAllAsync(default), Times.Once());
        _variableGroupAdapter.Verify(x => x.Setup(organization, project, pat), Times.Once());
    }

    [Test]
    public async Task GetAsync_Works_well_2()
    {
        // Arrange
        var organization = "Organization1";
        var pat = "WtxMFit1uz1k64u527mB";
        var project = "Project1";

        var variableRequest = TestSampleData.GetVariableRequest(organization, pat, project);

        var variableGroupEntity = TestSampleData.GetVariableGroupEntity();
        var variableGroupResponse = TestSampleData.GetVariableGroupGetResponses();

        _variableGroupAdapter.Setup(x => x.Setup(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        _variableGroupAdapter.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(variableGroupEntity);

        // Act
        var result = await _controller.GetAsync(variableRequest, default);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((VariableGroupResponses)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(variableGroupResponse);

        _variableGroupAdapter.Verify(x => x.GetAllAsync(default), Times.Once());
        _variableGroupAdapter.Verify(x => x.Setup(organization, project, pat), Times.Once());
    }

    [Test]
    public async Task UpdateAsync_Works_well()
    {
        // Arrange
        var organization = "Organization1";
        var pat = "WtxMFit1uz1k64u527mB";
        var project = "Project1";
        string valueFilter = null!;
        var newValue = "newValue";
        var statusResult = Status.Success;

        var variableRequest = TestSampleData.GetVariableUpdateRequest(organization, pat, project, valueFilter, newValue);

        var variableGroupEntity1 = TestSampleData.GetVariableGroupEntity();
        var variableGroupEntity2 = TestSampleData.GetVariableGroupEntity(newValue);
        var variableGroupResponse = TestSampleData.GetVariableGroupGetResponses(newValue);

        _variableGroupAdapter.Setup(x => x.Setup(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

        _variableGroupAdapter.Setup(x => x.UpdateAsync(It.IsAny<VariableGroupParameters>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(statusResult);

        _variableGroupAdapter.SetupSequence(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(variableGroupEntity1)
            .ReturnsAsync(variableGroupEntity2);

        // Act
        var result = await _controller.UpdateAsync(variableRequest, default);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((VariableGroupResponses)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(variableGroupResponse);

        _variableGroupAdapter.Verify(x => x.GetAllAsync(default), Times.Exactly(2));
        _variableGroupAdapter.Verify(x => x.UpdateAsync(It.IsAny<VariableGroupParameters>(), It.IsAny<int>(), default), Times.Exactly(2));
        _variableGroupAdapter.Verify(x => x.Setup(organization, project, pat), Times.Once());
    }

    [Test]
    public async Task AddAsync_Works_well()
    {
        // Arrange
        var organization = "Organization1";
        var pat = "WtxMFit1uz1k64u527mB";
        var project = "Project1";
        string valueFilter = null!;
        var newKey = "Test1";
        var newValue = "Test1";
        var statusResult = Status.Success;

        var variableRequest = TestSampleData.GetVariableAddRequest(organization, pat, project, valueFilter, newKey, newValue);

        var variableGroupEntity1 = TestSampleData.GetVariableGroupEntity();
        var variableGroupEntity2 = TestSampleData.GetVariableGroupEntity(newKey, newValue);
        var variableGroupResponse = TestSampleData.GetVariableGroupGetResponses(newKey, newValue);

        _variableGroupAdapter.Setup(x => x.Setup(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

        _variableGroupAdapter.Setup(x => x.UpdateAsync(It.IsAny<VariableGroupParameters>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(statusResult);

        _variableGroupAdapter.SetupSequence(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(variableGroupEntity1)
            .ReturnsAsync(variableGroupEntity2);

        // Act
        var result = await _controller.AddAsync(variableRequest, default);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((VariableGroupResponses)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(variableGroupResponse);

        _variableGroupAdapter.Verify(x => x.GetAllAsync(default), Times.Exactly(2));
        _variableGroupAdapter.Verify(x => x.UpdateAsync(It.IsAny<VariableGroupParameters>(), It.IsAny<int>(), default), Times.Exactly(2));
        _variableGroupAdapter.Verify(x => x.Setup(organization, project, pat), Times.Once());
    }

    [Test]
    public async Task DeleteAsync_Works_well()
    {
        // Arrange
        var organization = "Organization1";
        var pat = "WtxMFit1uz1k64u527mB";
        var project = "Project1";
        var keyFilter = "Test1";
        var entityValue = "Value1";
        var statusResult = Status.Success;

        var variableRequest = TestSampleData.GetVariableRequest(organization, pat, project, keyFilter, null!);

        var variableGroupEntity1 = TestSampleData.GetVariableGroupEntity(keyFilter, entityValue);
        var variableGroupEntity2 = TestSampleData.GetVariableGroupEntityAfterDelete();
        var variableGroupResponse = TestSampleData.GetVariableGroupGetResponsesAfterDelete();

        _variableGroupAdapter.Setup(x => x.Setup(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));

        _variableGroupAdapter.Setup(x => x.UpdateAsync(It.IsAny<VariableGroupParameters>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(statusResult);

        _variableGroupAdapter.SetupSequence(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(variableGroupEntity1)
            .ReturnsAsync(variableGroupEntity2);

        // Act
        var result = await _controller.DeleteAsync(variableRequest, default);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((VariableGroupResponses)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(variableGroupResponse);

        _variableGroupAdapter.Verify(x => x.GetAllAsync(default), Times.Exactly(2));
        _variableGroupAdapter.Verify(x => x.UpdateAsync(It.IsAny<VariableGroupParameters>(), It.IsAny<int>(), default), Times.Exactly(2));
        _variableGroupAdapter.Verify(x => x.Setup(organization, project, pat), Times.Once());
    }
}
