using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VGManager.Adapter.Models.Models;
using VGManager.Api.Endpoints.GitRepository.Request;
using VGManager.Services.Interfaces;
using VGManager.Services.Models.GitRepositories;

namespace VGManager.Api.Endpoints.GitRepository;

[Route("api/[controller]")]
[ApiController]
[EnableCors("_allowSpecificOrigins")]
public class GitRepositoryController(IGitRepositoryService gitRepositoryService, IMapper mapper) : ControllerBase
{
    [HttpPost(Name = "repositories")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AdapterResponseModel<IEnumerable<GitRepositoryResult>>>> GetAsync(
        [FromBody] GitRepositoryBaseRequest request,
        CancellationToken cancellationToken
    )
    {
        var gitRepositories = await gitRepositoryService.GetAllAsync(
            request.Organization,
            request.Project,
            request.PAT,
            cancellationToken
            );

        var result = new AdapterResponseModel<IEnumerable<GitRepositoryResult>>()
        {
            Status = gitRepositories.Status,
            Data = gitRepositories.Data
        };

        return Ok(result);
    }

    [HttpPost("Variables", Name = "Variables")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AdapterResponseModel<IEnumerable<string>>>> GetVariablesAsync(
        [FromBody] GitRepositoryVariablesRequest request,
        CancellationToken cancellationToken
    )
    {
        var model = mapper.Map<GitRepositoryModel>(request);
        var variablesResult = await gitRepositoryService.GetVariablesFromConfigAsync(model, cancellationToken);
        var result = new AdapterResponseModel<IEnumerable<string>>()
        {
            Status = variablesResult.Status,
            Data = variablesResult.Data
        };
        return Ok(result);
    }
}
