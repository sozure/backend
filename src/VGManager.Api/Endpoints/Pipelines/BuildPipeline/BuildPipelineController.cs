using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VGManager.Api.Common;
using VGManager.Api.Endpoints.Pipelines.BuildPipeline;
using VGManager.Models.Models;
using VGManager.Models.StatusEnums;
using VGManager.Services.Interfaces;

namespace VGManager.Api.Endpoints.Pipelines.Build;

[Route("api/[controller]")]
[ApiController]
[EnableCors("_allowSpecificOrigins")]
public class BuildPipelineController : ControllerBase
{
    private readonly IBuildPipelineService _buildPipelineService;

    public BuildPipelineController(IBuildPipelineService buildPipelineService)
    {
        _buildPipelineService = buildPipelineService;
    }

    [HttpPost("GetAll", Name = "getall")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AdapterResponseModel<IEnumerable<Dictionary<string, string>>>>> GetAll(
        [FromBody] ExtendedBasicRequest request,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var pipelines = await _buildPipelineService.GetBuildPipelinesAsync(
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
        [FromBody] BuildPipelineRequest request,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var status = await _buildPipelineService.RunBuildPipelineAsync(
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
}
