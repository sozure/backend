using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VGManager.Adapter.Models.Models;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Api.Endpoints.Pipelines.Release.Request;
using VGManager.Services.Interfaces;

namespace VGManager.Api.Endpoints.Pipelines.Release;

[Route("api/[controller]")]
[ApiController]
[EnableCors("_allowSpecificOrigins")]
public class ReleasePipelineController(IReleasePipelineService releasePipelineService) : ControllerBase
{
    [HttpPost("GetEnvironments", Name = "Environments")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AdapterResponseModel<IEnumerable<string>>>> GetEnvironmentsAsync(
        [FromBody] ReleasePipelineRequest request,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var (status, environments) = await releasePipelineService.GetEnvironmentsAsync(
                request.Organization,
                request.PAT,
                request.Project,
                request.RepositoryName,
                request.ConfigFile,
                cancellationToken
                );

            return Ok(new AdapterResponseModel<IEnumerable<string>>()
            {
                Status = status,
                Data = environments
            });
        }
        catch (Exception)
        {
            return Ok(new AdapterResponseModel<IEnumerable<string>>()
            {
                Status = AdapterStatus.Unknown,
                Data = Enumerable.Empty<string>()
            });
        }
    }

    [HttpPost("GetVariableGroups", Name = "VariableGroups")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AdapterResponseModel<IEnumerable<Dictionary<string, string>>>>> GetVariableGroupsAsync(
        [FromBody] ReleasePipelineRequest request,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var (status, variableGroups) = await releasePipelineService.GetVariableGroupsAsync(
                request.Organization,
                request.PAT,
                request.Project,
                request.RepositoryName,
                request.ConfigFile,
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


            return Ok(new AdapterResponseModel<IEnumerable<Dictionary<string, string>>>()
            {
                Status = status,
                Data = result
            });
        }
        catch (Exception)
        {
            return Ok(new AdapterResponseModel<IEnumerable<Dictionary<string, string>>>()
            {
                Status = AdapterStatus.Unknown,
                Data = Enumerable.Empty<Dictionary<string, string>>()
            });
        }
    }

    [HttpPost("GetProjects", Name = "Projects")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AdapterResponseModel<IEnumerable<string>>>> GetProjectsWithReleasePipelineAsync(
        [FromBody] ProjectsWithReleasePipelineRequest request,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var (status, projects) = await releasePipelineService.GetProjectsWithReleasePipelineAsync(
                request.Organization,
                request.PAT,
                request.Projects,
                request.RepositoryName,
                request.ConfigFile,
                cancellationToken
                );

            return Ok(new AdapterResponseModel<IEnumerable<string>>()
            {
                Status = status,
                Data = projects
            });
        }
        catch (Exception)
        {
            return Ok(new AdapterResponseModel<IEnumerable<string>>()
            {
                Status = AdapterStatus.Unknown,
                Data = Enumerable.Empty<string>()
            });
        }
    }
}
