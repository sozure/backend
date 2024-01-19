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
public class GitRepositoryController : ControllerBase
{
    private readonly IGitRepositoryService _gitRepositoryService;
    private readonly IMapper _mapper;

    public GitRepositoryController(IGitRepositoryService gitRepositoryService, IMapper mapper)
    {
        _gitRepositoryService = gitRepositoryService;
        _mapper = mapper;
    }

    [HttpPost(Name = "repositories")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AdapterResponseModel<IEnumerable<GitRepositoryResult>>>> GetAsync(
        [FromBody] GitRepositoryBaseRequest request,
        CancellationToken cancellationToken
    )
    {
        var gitRepositories = await _gitRepositoryService.GetAllAsync(
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
        var model = _mapper.Map<GitRepositoryModel>(request);
        var variablesResult = await _gitRepositoryService.GetVariablesFromConfigAsync(model, cancellationToken);
        var result = new AdapterResponseModel<IEnumerable<string>>()
        {
            Status = variablesResult.Status,
            Data = variablesResult.Data
        };
        return Ok(result);
    }
}
