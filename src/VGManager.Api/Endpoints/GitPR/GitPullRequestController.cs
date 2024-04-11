using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VGManager.Adapter.Models.Models;
using VGManager.Adapter.Models.Requests;
using VGManager.Adapter.Models.Response;
using VGManager.Services.Interfaces;

namespace VGManager.Api.Endpoints.GitPR;

[Route("api/[controller]")]
[ApiController]
[EnableCors("_allowSpecificOrigins")]
public class GitPullRequestController(IGitPullRequestService gitPullRequestService, IMapper mapper) : ControllerBase
{

    [HttpPost("Get", Name = "getprs")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AdapterResponseModel<IEnumerable<GitPRResponse>>>> GetAsync(
        [FromBody] GitPRRequest request,
        CancellationToken cancellationToken
    )
    {
        var gitPullRequests = await gitPullRequestService.GetPRsAsync(request, cancellationToken);
        return Ok(gitPullRequests);
    }

    [HttpPost("Create", Name = "createpr")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AdapterResponseModel<bool>>> CreatePullRequestAsync(
        [FromBody] CreatePRRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await gitPullRequestService.CreatePullRequestAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("CreateMultiple", Name = "createprs")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AdapterResponseModel<IEnumerable<GitPRResponse>>>> CreatePullRequestsAsync(
        [FromBody] CreatePRsRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await gitPullRequestService.CreatePullRequestsAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("Approve", Name = "approve")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AdapterResponseModel<IEnumerable<GitPRResponse>>>> ApprovePullRequestsAsync(
        [FromBody] ApprovePRsRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await gitPullRequestService.ApprovePullRequestsAsync(request, cancellationToken);
        return Ok(result);
    }
}
