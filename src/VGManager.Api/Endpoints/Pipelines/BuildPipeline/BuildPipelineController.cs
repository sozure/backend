using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VGManager.Adapter.Models.Models;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Api.Common;
using VGManager.Api.Endpoints.Pipelines.BuildPipeline;
using VGManager.Services.Interfaces;

namespace VGManager.Api.Endpoints.Pipelines.Build;

[Route("api/[controller]")]
[ApiController]
[EnableCors("_allowSpecificOrigins")]
public class BuildPipelineController(IBuildPipelineService buildPipelineService) : ControllerBase
{
    [HttpPost("GetRepositoryId", Name = "getrepositoryid")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AdapterResponseModel<string>>> GetRepositoryIdAsync(
        [FromBody] BuildPipelineRequest request,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var id = await buildPipelineService.GetRepositoryIdByBuildDefinitionAsync(
                request.Organization,
                request.PAT,
                request.Project,
                request.DefinitionId,
                cancellationToken
                );
            return Ok(new AdapterResponseModel<Guid>()
            {
                Status = AdapterStatus.Success,
                Data = id
            });
        }
        catch (Exception)
        {
            return Ok(new AdapterResponseModel<Guid>()
            {
                Status = AdapterStatus.Unknown,
                Data = Guid.Empty
            });
        }
    }

    [HttpPost("GetAll", Name = "getall")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AdapterResponseModel<IEnumerable<Dictionary<string, string>>>>> GetAllAsync(
        [FromBody] ExtendedBasicRequest request,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var pipelines = await buildPipelineService.GetBuildPipelinesAsync(
                request.Organization,
                request.PAT,
                request.Project,
                cancellationToken
                );
            return Ok(new AdapterResponseModel<IEnumerable<Dictionary<string, string>>>()
            {
                Status = AdapterStatus.Success,
                Data = pipelines
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

    [HttpPost("Run", Name = "run")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AdapterStatus>> RunBuildPipelineAsync(
        [FromBody] RunBuildRequest request,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var status = await buildPipelineService.RunBuildPipelineAsync(
                request.Organization,
                request.PAT,
                request.Project,
                request.DefinitionId,
                request.SourceBranch,
                cancellationToken
                );
            return Ok(status);
        }
        catch (Exception)
        {
            return Ok(AdapterStatus.Unknown);
        }
    }

    [HttpPost("RunAll", Name = "runall")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AdapterStatus>> RunBuildPipelinesAsync(
        [FromBody] RunBuildPipelinesRequest request,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var status = await buildPipelineService.RunBuildPipelinesAsync(
                request.Organization,
                request.PAT,
                request.Project,
                request.Pipelines,
                cancellationToken
                );

            return Ok(status);
        }
        catch (Exception)
        {
            return Ok(AdapterStatus.Unknown);
        }
    }
}
