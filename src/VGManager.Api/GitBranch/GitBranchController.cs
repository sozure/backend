using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VGManager.Api.Common;
using VGManager.Models.Models;
using VGManager.Models.StatusEnums;
using VGManager.Services.Interfaces;

namespace VGManager.Api.Branch;

[Route("api/[controller]")]
[ApiController]
[EnableCors("_allowSpecificOrigins")]
public class GitBranchController : ControllerBase
{

    private readonly IGitBranchService _gitBranchService;

    public GitBranchController(IGitBranchService gitBranchService)
    {
        _gitBranchService = gitBranchService;
    }

    [HttpPost(Name = "branches")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AdapterResponseModel<IEnumerable<string>>>> GetAsync(
        [FromBody] GitBasicRequest request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            (var status, var branches) = await _gitBranchService.GetAllAsync(
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
}
