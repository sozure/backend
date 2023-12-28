using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VGManager.Api.Projects.Responses;
using VGManager.Api.Projects;
using VGManager.Services;
using VGManager.Services.Interfaces;
using VGManager.Services.Models.Projects;

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

    [HttpPost("Get", Name = "getgitrepositories")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GitRepositoryResponse>> GetAsync(
        [FromBody] GitRepositoryRequest request,
        CancellationToken cancellationToken
    )
    {
        var gitRepositories = await _gitRepositoryService.GetAllAsync(
            request.Organization, 
            request.Project, 
            request.PAT, 
            cancellationToken
            );

        var result = _mapper.Map<GitRepositoryResponse>(gitRepositories);
        return Ok(result);
    }
}
