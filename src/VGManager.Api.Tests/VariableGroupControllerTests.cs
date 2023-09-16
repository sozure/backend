using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.DistributedTask.WebApi;
using VGManager.Api.Controllers;
using VGManager.Api.MapperProfiles;
using VGManager.Api.VariableGroup.Response;
using VGManager.Api.VariableGroups.Request;
using VGManager.Api.VariableGroups.Response;
using VGManager.AzureAdapter.Entities;
using VGManager.AzureAdapter.Interfaces;
using VGManager.Services;
using ProjectProfile = VGManager.Services.MapperProfiles.ProjectProfile;
using VariableGroupE = Microsoft.TeamFoundation.DistributedTask.WebApi.VariableGroup;

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

        var variableRequest = GetVariableRequest(organization, pat, project, valueFilter);

        var variableGroupEntity = GetVariableGroupEntity();
        var variableGroupResponse = GetVariableGroupGetResponses();

        _variableGroupAdapter.Setup(x => x.Setup(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        _variableGroupAdapter.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(variableGroupEntity);

        // Act
        var result = await _controller.GetAsync(variableRequest, default);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((VariableGroupGetResponses)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(variableGroupResponse);

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

        var variableRequest = GetVariableRequest(organization, pat, project);

        var variableGroupEntity = GetVariableGroupEntity();
        var variableGroupResponse = GetVariableGroupGetResponses();

        _variableGroupAdapter.Setup(x => x.Setup(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()));
        _variableGroupAdapter.Setup(x => x.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(variableGroupEntity);

        // Act
        var result = await _controller.GetAsync(variableRequest, default);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((VariableGroupGetResponses)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(variableGroupResponse);

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

        var variableRequest = GetVariableUpdateRequest(organization, pat, project, valueFilter, newValue);

        var variableGroupEntity1 = GetVariableGroupEntity();
        var variableGroupEntity2 = GetVariableGroupEntity(newValue);
        var variableGroupResponse = GetVariableGroupGetResponses(newValue);

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
        ((VariableGroupGetResponses)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(variableGroupResponse);

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

        var variableRequest = GetVariableAddRequest(organization, pat, project, valueFilter, newKey, newValue);

        var variableGroupEntity1 = GetVariableGroupEntity();
        var variableGroupEntity2 = GetVariableGroupEntity(newKey, newValue);
        var variableGroupResponse = GetVariableGroupGetResponses(newKey, newValue);

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
        ((VariableGroupGetResponses)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(variableGroupResponse);

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

        var variableRequest = GetVariableDeleteRequest(organization, pat, project, keyFilter);

        var variableGroupEntity1 = GetVariableGroupEntity(keyFilter, entityValue);
        var variableGroupEntity2 = GetVariableGroupEntityAfterDelete();
        var variableGroupResponse = GetVariableGroupGetResponsesAfterDelete();

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
        ((VariableGroupGetResponses)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(variableGroupResponse);

        _variableGroupAdapter.Verify(x => x.GetAllAsync(default), Times.Exactly(2));
        _variableGroupAdapter.Verify(x => x.UpdateAsync(It.IsAny<VariableGroupParameters>(), It.IsAny<int>(), default), Times.Exactly(2));
        _variableGroupAdapter.Verify(x => x.Setup(organization, project, pat), Times.Once());
    }

    private static VariableGroupDeleteRequest GetVariableDeleteRequest(string organization, string pat, string project, string keyFilter)
    {
        return new VariableGroupDeleteRequest
        {
            Organization = organization,
            PAT = pat,
            Project = project,
            VariableGroupFilter = "neptun",
            KeyFilter = keyFilter,
            ValueFilter = null!,
            ContainsSecrets = false
        };
    }

    private static VariableGroupAddRequest GetVariableAddRequest(
        string organization,
        string pat,
        string project,
        string valueFilter,
        string newKey,
        string newValue
        )
    {
        return new VariableGroupAddRequest
        {
            Organization = organization,
            PAT = pat,
            Project = project,
            VariableGroupFilter = "neptun",
            KeyFilter = null!,
            ValueFilter = valueFilter,
            Key = newKey,
            Value = newValue
        };
    }

    private static VariableGroupUpdateRequest GetVariableUpdateRequest(
        string organization,
        string pat,
        string project,
        string valueFilter,
        string newValue
        )
    {
        return new VariableGroupUpdateRequest
        {
            Organization = organization,
            PAT = pat,
            Project = project,
            VariableGroupFilter = "neptun",
            KeyFilter = "key",
            ValueFilter = valueFilter,
            ContainsSecrets = false,
            NewValue = newValue
        };
    }

    private static VariableGroupGetRequest GetVariableRequest(string organization, string pat, string project, string valueFilter)
    {
        return new VariableGroupGetRequest
        {
            Organization = organization,
            PAT = pat,
            Project = project,
            VariableGroupFilter = "neptun",
            KeyFilter = "key",
            ValueFilter = valueFilter,
            ContainsSecrets = false
        };
    }

    private static VariableGroupGetRequest GetVariableRequest(string organization, string pat, string project)
    {
        return new VariableGroupGetRequest
        {
            Organization = organization,
            PAT = pat,
            Project = project,
            VariableGroupFilter = "neptun",
            KeyFilter = "key",
            ContainsSecrets = false
        };
    }

    private static VariableGroupEntity GetVariableGroupEntity()
    {
        return new VariableGroupEntity
        {
            Status = Status.Success,
            VariableGroups = new List<VariableGroupE>
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
    }

    private static VariableGroupEntity GetVariableGroupEntityAfterDelete()
    {
        return new VariableGroupEntity
        {
            Status = Status.Success,
            VariableGroups = new List<VariableGroupE>()
        };
    }

    private static VariableGroupEntity GetVariableGroupEntity(string value)
    {
        return new VariableGroupEntity
        {
            Status = Status.Success,
            VariableGroups = new List<VariableGroupE>
            {
                new()
                {
                    Name = "NeptunAdapter",
                    Variables = new Dictionary<string, VariableValue>
                    {
                        ["Key123"] = new()
                        {
                            Value = value
                        },
                        ["Key456"] = new()
                        {
                            Value = value
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
                            Value = value
                        }
                    }
                },
            }
        };
    }

    private static VariableGroupEntity GetVariableGroupEntity(string key, string value)
    {
        return new VariableGroupEntity
        {
            Status = Status.Success,
            VariableGroups = new List<VariableGroupE>
            {
                new()
                {
                    Name = "NeptunAdapter",
                    Variables = new Dictionary<string, VariableValue>
                    {
                        [key] = new()
                        {
                            Value = value
                        }
                    }
                },
                new()
                {
                    Name = "NeptunApi",
                    Variables = new Dictionary<string, VariableValue>
                    {
                        [key] = new()
                        {
                            Value = value
                        }
                    }
                },
            }
        };
    }

    private static VariableGroupGetResponses GetVariableGroupGetResponses(string key, string value)
    {
        var list = new List<VariableGroupGetResponse>()
        {
                new()
                {
                    Project = "Project1",
                    VariableGroupName = "NeptunAdapter",
                    VariableGroupKey = key,
                    VariableGroupValue = value
                },
                new()
                {
                    Project = "Project1",
                    VariableGroupName = "NeptunApi",
                    VariableGroupKey = key,
                    VariableGroupValue = value
                }
        };

        var result = new List<VariableGroupGetBaseResponse>();

        foreach (var item in list)
        {
            result.Add(item);
        }

        return new VariableGroupGetResponses
        {
            Status = Status.Success,
            VariableGroups = result
        };
    }

    private static VariableGroupGetResponses GetVariableGroupGetResponses(string value)
    {
        var list = new List<VariableGroupGetResponse>()
        {
                new()
                {
                    Project = "Project1",
                    VariableGroupName = "NeptunAdapter",
                    VariableGroupKey = "Key123",
                    VariableGroupValue = value
                },
                new()
                {
                    Project = "Project1",
                    VariableGroupName = "NeptunAdapter",
                    VariableGroupKey = "Key456",
                    VariableGroupValue = value
                },
                new()
                {
                    Project = "Project1",
                    VariableGroupName = "NeptunApi",
                    VariableGroupKey = "Key789",
                    VariableGroupValue = value
                }
        };

        var result = new List<VariableGroupGetBaseResponse>();

        foreach (var item in list)
        {
            result.Add(item);
        }

        return new VariableGroupGetResponses
        {
            Status = Status.Success,
            VariableGroups = result
        };
    }

    private static VariableGroupGetResponses GetVariableGroupGetResponses()
    {
        var list = new List<VariableGroupGetResponse>()
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
        };

        var result = new List<VariableGroupGetBaseResponse>();

        foreach (var item in list)
        {
            result.Add(item);
        }

        return new VariableGroupGetResponses
        {
            Status = Status.Success,
            VariableGroups = result
        };
    }

    private static VariableGroupGetResponses GetVariableGroupGetResponsesAfterDelete()
    {
        return new VariableGroupGetResponses
        {
            Status = Status.Success,
            VariableGroups = new List<VariableGroupGetBaseResponse>()
        };
    }
}
