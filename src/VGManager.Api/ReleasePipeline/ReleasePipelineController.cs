using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VGManager.Api.ReleasePipeline.Request;
using VGManager.Api.ReleasePipeline.Response;
using VGManager.Services.Interfaces;

namespace VGManager.Api.ReleasePipeline;

[Route("api/[controller]")]
[ApiController]
[EnableCors("_allowSpecificOrigins")]
public class ReleasePipelineController : ControllerBase
{
    private readonly IReleasePipelineService _releasePipelineService;
    private readonly IMapper _mapper;

    public ReleasePipelineController(IReleasePipelineService releasePipelineService, IMapper mapper)
    {
        _releasePipelineService = releasePipelineService;
        _mapper = mapper;
    }

    [HttpPost("GetEnvironments", Name = "Environments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ReleasePipelineEnvResponse>> GetEnvironmentsAsync(
        [FromBody] ReleasePipelineRequest request,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var (status, environments) = await _releasePipelineService.GetEnvironmentsAsync(
                request.Organization,
                request.PAT,
                request.Project,
                request.RepositoryName,
                cancellationToken
                );

            return Ok(new ReleasePipelineEnvResponse()
            {
                Status = status,
                Environments = environments
            });
        }
        catch (Exception)
        {
            return Ok(new ReleasePipelineEnvResponse()
            {
                Status = AdapterStatus.Unknown,
                Environments = Enumerable.Empty<string>()
            });
        }
    }

    [HttpPost("GetVariableGroups", Name = "VariableGroups")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ReleasePipelineEnvResponse>> GetVariableGroupsAsync(
        [FromBody] ReleasePipelineRequest request,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var (status, variableGroups) = await _releasePipelineService.GetVariableGroupsAsync(
                request.Organization,
                request.PAT,
                request.Project,
                request.RepositoryName,
                cancellationToken
                );

            var result = new List<Dictionary<string, string>>();

            foreach (var (name, type) in variableGroups)
            {
                var dictionary = new Dictionary<string, string>
                {
                    { "Name", name },
                    { "Type", type }
                };
                result.Add(dictionary);
            }


            return Ok(new ReleasePipelineVGResponse()
            {
                Status = status,
                VariableGroups = result
            });
        }
        catch (Exception)
        {
            return Ok(new ReleasePipelineVGResponse()
            {
                Status = AdapterStatus.Unknown,
                VariableGroups = Enumerable.Empty<Dictionary<string, string>>()
            });
        }
    }

    [HttpPost("GetProjects", Name = "Projects")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProjectsWithCorrespondingReleasePipelineResponse>> GetProjectsWithCorrespondingReleasePipelineAsync(
        [FromBody] ProjectsWithCorrespondingReleasePipelineRequest request,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var (status, projects) = await _releasePipelineService.GetProjectsWhichHaveCorrespondingReleasePipelineAsync(
                request.Organization,
                request.PAT,
                request.Projects,
                request.RepositoryName,
                cancellationToken
                );

            return Ok(new ProjectsWithCorrespondingReleasePipelineResponse()
            {
                Status = status,
                Projects = projects
            });
        }
        catch (Exception)
        {
            return Ok(new ProjectsWithCorrespondingReleasePipelineResponse()
            {
                Status = AdapterStatus.Unknown,
                Projects = Enumerable.Empty<string>()
            });
        }
    }
}
