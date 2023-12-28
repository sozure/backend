using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VGManager.Api.GitRepository.Request;
using VGManager.Services.Interfaces;

namespace VGManager.Api.GitRepository;

[Route("api/[controller]")]
[ApiController]
[EnableCors("_allowSpecificOrigins")]
public class GitRepositoryController: ControllerBase
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
    public async Task<ActionResult<GitRepositoryResponses>> GetAsync(
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

        var result = _mapper.Map<GitRepositoryResponses>(gitRepositories);
        return Ok(result);
    }

    [HttpPost("Variables", Name = "Variables")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GitRepositoryResponses>> GetVariablesAsync(
        [FromBody] GitRepositoryVariablesRequest request,
        CancellationToken cancellationToken
    )
    {
        var variablesFromConfigs = await _gitRepositoryService.GetVariablesFromConfigAsync(
            request.Organization,
            request.Project,
            request.PAT,
            request.GitRepositoryId,
            request.FilePath,
            request.Delimiter,
            cancellationToken
            );

        //var result = _mapper.Map<GitRepositoryResponse>(variablesFromConfigs);
        //return Ok(result);
        return Ok(variablesFromConfigs);
    }
}
