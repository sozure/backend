using AutoMapper;
using VGManager.Api.Controllers;
using VGManager.Services;
using VGManager.Api.MapperProfiles;
using VGManager.AzureAdapter.Interfaces;
using Microsoft.Extensions.Logging;
using ProjectProfile = VGManager.Services.MapperProfiles.ProjectProfile;
using VGManager.Api.VariableGroups.Request;
using VGManager.AzureAdapter.Entities;
using Microsoft.TeamFoundation.DistributedTask.WebApi;
using Microsoft.AspNetCore.Mvc;
using VGManager.Api.Projects.Responses;
using VGManager.Api.VariableGroups.Response;

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
    public async Task GetAsync_Works_well()
    {
        // Arrange
        var organization = "Organization1";
        var pat = "WtxMFit1uz1k64u527mB";
        var project = "Project1";

        var variableRequest = new VariableGroupGetRequest
        {
            Organization = organization,
            PAT = pat,
            Project = project,
            VariableGroupFilter = "Neptun",
            KeyFilter = "Key",
            ValueFilter = "Value",
            ContainsSecrets = false
        };

        var variableGroupEntity = new VariableGroupEntity
        {
            Status = Status.Success,
            VariableGroups = new List<VariableGroup>
            {
                new()
                {
                    Name = "NeptunAdapter",
                    Variables = new Dictionary<string, VariableValue>
                    {
                        ["Key123"] = new()
                        {
                            Value = "Value123"
                        },
                        ["Key456"] = new()
                        {
                            Value = "Value456"
                        }
                    }
                },
                new()
                {
                    Name = "NeptunApi",
                    Variables = new Dictionary<string, VariableValue>
                    {
                        ["Key789"] = new()
                        {
                            Value = "Value789"
                        },
                        ["Kec"] = new()
                        {
                            Value = "Valuc"
                        }
                    }
                },
            }
        };

        var variableGroupResponse = new VariableGroupGetResponses
        {
            Status = Status.Success,
            VariableGroups = new List<VariableGroupGetResponse>()
            {
                new()
                {
                    Project = "Project1",
                    VariableGroupName = "NeptunAdapter",
                    VariableGroupKey = "Key123",
                    VariableGroupValue = "Value123"
                },
                new()
                {
                    Project = "Project1",
                    VariableGroupName = "NeptunAdapter",
                    VariableGroupKey = "Key456",
                    VariableGroupValue = "Value456"
                },
                new()
                {
                    Project = "Project1",
                    VariableGroupName = "NeptunApi",
                    VariableGroupKey = "Key789",
                    VariableGroupValue = "Value789"
                }
            }
        };

        _variableGroupAdapter.Setup(x => x.Setup(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        _variableGroupAdapter.Setup(x => x.GetAll(It.IsAny<CancellationToken>()))
            .ReturnsAsync(variableGroupEntity);

        // Act
        var result = await _controller.GetAsync(variableRequest, default);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((VariableGroupGetResponses)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(variableGroupResponse);

        _variableGroupAdapter.Verify(x => x.GetAll(default), Times.Once());
        _variableGroupAdapter.Verify(x => x.Setup(organization, project, pat), Times.Once());
    }
}
