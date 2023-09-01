using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VGManager.Services.Interfaces;
using VGManager.Services.Models;

namespace VGManager.Api.Projects;

[ApiVersion("1.0")]
[Route("api/[controller]")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly IMapper _mapper;

    public ProjectController(IProjectService projectService, IMapper mapper)
    {
        _projectService = projectService;
        _mapper = mapper;
    }

    [HttpGet(Name = "getprojects")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<ProjectResponse>>> GetAsync(
        [FromQuery] ProjectRequest request,
        CancellationToken cancellationToken
    )
    {
        var projectModel = _mapper.Map<ProjectModel>(request);

        var projects = await _projectService.GetProjects(projectModel, cancellationToken);

        var result = projects.Select(_mapper.Map<ProjectResponse>);
        return Ok(result);
    }
}
