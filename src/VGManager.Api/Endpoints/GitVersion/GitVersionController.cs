using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VGManager.Adapter.Models.Models;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Api.Common;
using VGManager.Api.Endpoints.GitVersion;
using VGManager.Services.Interfaces;
using VGManager.Services.Models;

namespace VGManager.Api.Endpoints.GitBranch;

[Route("api/[controller]")]
[ApiController]
[EnableCors("_allowSpecificOrigins")]
public class GitVersionController(IGitVersionService gitBranchService, IMapper mapper) : ControllerBase
{
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
            (var status, var branches) = await gitBranchService.GetBranchesAsync(
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
            (var status, var tags) = await gitBranchService.GetTagsAsync(
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

    [HttpPost("LatestTags", Name = "latesttags")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AdapterResponseModel<Dictionary<string, string>>>> GetLatestTagsAsync(
        [FromBody] GitLatestTagsRequest request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var model = mapper.Map<GitLatestTagsEntity>(request);
            var result = await gitBranchService.GetLatestTagsAsync(model, cancellationToken);
            return Ok(result);
        }
        catch (Exception)
        {
            return Ok(new AdapterResponseModel<Dictionary<string, string>>
            {
                Status = AdapterStatus.Unknown,
                Data = []
            });
        }
    }

    [HttpPost("Tag/Create", Name = "tagcreation")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AdapterResponseModel<string>>> CreateTagAsync(
        [FromBody] CreateTagEntity request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            (var status, var tag) = await gitBranchService.CreateTagAsync(
                request,
                cancellationToken
                );

            return Ok(new AdapterResponseModel<string>
            {
                Status = status,
                Data = tag
            });
        }
        catch (Exception)
        {
            return Ok(new AdapterResponseModel<string>
            {
                Status = AdapterStatus.Unknown,
                Data = string.Empty
            });
        }
    }
}
