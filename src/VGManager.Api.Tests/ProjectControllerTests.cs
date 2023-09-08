using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.TeamFoundation.Core.WebApi;
using VGManager.Api.Projects;
using VGManager.Api.Projects.Responses;
using VGManager.Api.VariableGroups.Response;
using VGManager.AzureAdapter.Entities;
using VGManager.AzureAdapter.Interfaces;
using VGManager.Services;
using ApiProjectProfile = VGManager.Api.MapperProfiles.ProjectProfile;
using ServiceProjectProfile = VGManager.Services.MapperProfiles.ProjectProfile;

namespace VGManager.Api.Tests;

[TestFixture]
public class ProjectControllerTests
{
    private ProjectController _controller;
    private Mock<IProjectAdapter> _adapter;

    [SetUp]
    public void Setup()
    {
        var apiMapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(ApiProjectProfile));
        });

        var serviceMapperConfiguration = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(ServiceProjectProfile));
        });

        var apiMapper = apiMapperConfiguration.CreateMapper();
        var serviceMapper = serviceMapperConfiguration.CreateMapper();

        _adapter = new Mock<IProjectAdapter>(MockBehavior.Strict);
        var projectService = new ProjectService(_adapter.Object, serviceMapper);

        _controller = new(projectService, apiMapper);
    }

    [Test]
    public async Task GetAsync_Works_well()
    {
        // Arrange
        var organization = "Organization1";
        var url = $"https://dev.azure.com/{organization}";
        var pat = "WtxMFit1uz1k64u527mB";

        var request = new ProjectRequest
        {
            Organization = organization,
            PAT = pat
        };

        var projectEntity = new ProjectEntity
        {
            Status = Status.Success,
            Projects = new List<TeamProjectReference>()
            {
                new TeamProjectReference()
                {
                    Name = "Project1"
                },
                new TeamProjectReference()
                {
                    Name = "Project2"
                }
            }
        };

        var projectsResponse = new ProjectsResponse
        {
            Status = Status.Success,
            Projects = new List<ProjectResponse>()
            {
                new ProjectResponse()
                {
                    Name = "Project1"
                },
                new ProjectResponse()
                {
                    Name = "Project2"
                }
            }
        };

        _adapter.Setup(x => x.GetProjects(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ReturnsAsync(projectEntity);

        // Act
        var result = await _controller.GetAsync(request, default);

        // Assert
        result.Should().NotBeNull();
        result.Result.Should().BeOfType<OkObjectResult>();
        ((ProjectsResponse)((OkObjectResult)result.Result!).Value!).Should().BeEquivalentTo(projectsResponse);

        _adapter.Verify(x => x.GetProjects(url, pat, default), Times.Once);
    }
}
