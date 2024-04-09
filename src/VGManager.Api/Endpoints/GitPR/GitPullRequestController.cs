using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using VGManager.Adapter.Models.Models;
using VGManager.Services.Interfaces;
using VGManager.Services.Models;

namespace VGManager.Api.Endpoints.GitPR;

[Route("api/[controller]")]
[ApiController]
[EnableCors("_allowSpecificOrigins")]
public class GitPullRequestController(IGitPullRequestService gitPullRequestService, IMapper mapper) : ControllerBase
{

    [HttpPost("PRs", Name = "pullrequests")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AdapterResponseModel<IEnumerable<GitPRResponse>>>> GetPRsAsync(
        [FromBody] PRRequest request,
        CancellationToken cancellationToken
    )
    {
        var gitPullRequests = await gitPullRequestService.GetPRsAsync(request, cancellationToken);

        var result = new AdapterResponseModel<IEnumerable<GitPRResponse>>()
        {
            Status = gitPullRequests.Status,
            Data = gitPullRequests.Data
        };

        return Ok(result);
    }
}
