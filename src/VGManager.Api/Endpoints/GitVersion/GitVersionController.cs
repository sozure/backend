using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VGManager.Api.Common;
using VGManager.Models.Models;
using VGManager.Models.StatusEnums;
using VGManager.Services.Interfaces;

namespace VGManager.Api.Endpoints.GitBranch;

[Route("api/[controller]")]
[ApiController]
[EnableCors("_allowSpecificOrigins")]
public class GitVersionController : ControllerBase
{

    private readonly IGitVersionService _gitBranchService;

    public GitVersionController(IGitVersionService gitBranchService)
    {
        _gitBranchService = gitBranchService;
    }

    [HttpPost("Branches", Name = "branches")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AdapterResponseModel<IEnumerable<string>>>> GetBranchesAsync(
        [FromBody] GitBasicRequest request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            (var status, var branches) = await _gitBranchService.GetBranchesAsync(
                request.Organization,
                request.PAT,
                request.RepositoryId,
                cancellationToken
                );
            return Ok(new AdapterResponseModel<IEnumerable<string>>
            {
                Status = status,
                Data = branches
            });
        }
        catch (Exception)
        {
            return Ok(new AdapterResponseModel<IEnumerable<string>>
            {
                Status = AdapterStatus.Unknown,
                Data = Enumerable.Empty<string>()
            });
        }
    }

    [HttpPost("Tags", Name = "tags")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AdapterResponseModel<IEnumerable<string>>>> GetTagsAsync(
        [FromBody] GitBasicRequest request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            (var status, var tags) = await _gitBranchService.GetTagsAsync(
                request.Organization,
                request.PAT,
                new Guid(request.RepositoryId),
                cancellationToken
                );
            return Ok(new AdapterResponseModel<IEnumerable<string>>
            {
                Status = status,
                Data = tags
            });
        }
        catch (Exception)
        {
            return Ok(new AdapterResponseModel<IEnumerable<string>>
            {
                Status = AdapterStatus.Unknown,
                Data = Enumerable.Empty<string>()
            });
        }
    }
}
