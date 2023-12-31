using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using VGManager.Api.GitBranch;
using VGManager.AzureAdapter.Entities;
using VGManager.Services.Interfaces;

namespace VGManager.Api.Branch;

[Route("api/[controller]")]
[ApiController]
[EnableCors("_allowSpecificOrigins")]
public class GitBranchController: ControllerBase
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
    public async Task<ActionResult<GitBranchResponse>> GetAsync(
        [FromBody] GitBranchRequest request,
        CancellationToken cancellationToken
    )
    {
        (AdapterStatus status, IEnumerable<TfvcBranch> branches) = (AdapterStatus.None, null!);
        try
        {
            (status, branches) = await _gitBranchService.GetAllAsync(
                request.Organization,
                request.PAT,
                request.GitProject,
                cancellationToken
                );
            return Ok(new GitBranchResponse
            {
                Status = status,
                Branches = Enumerable.Empty<string>()
            });
        } catch (Exception)
        {
            status = AdapterStatus.Unknown;
            return Ok(new GitBranchResponse
            {
                Status = status,
                Branches = Enumerable.Empty<string>()
            });
        }
    }
}
