using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VGManager.Api.Endpoints.Pipelines.BuildPipeline;
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

    [HttpPost(Name = "Run")]
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
