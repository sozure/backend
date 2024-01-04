using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using VGManager.Api.Secret.Request;
using VGManager.Api.Secret.Response;
using VGManager.AzureAdapter.Entities;
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

    [HttpPost("GetEnvironments", Name = "GetEnvironments")]
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

    [HttpPost("GetVariableGroups", Name = "GetVariableGroups")]
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

            return Ok(new ReleasePipelineVGResponse()
            {
                Status = status,
                VariableGroups = variableGroups
            });
        }
        catch (Exception)
        {
            return Ok(new ReleasePipelineVGResponse()
            {
                Status = AdapterStatus.Unknown,
                VariableGroups = Enumerable.Empty<string>()
            });
        }
    }
}
