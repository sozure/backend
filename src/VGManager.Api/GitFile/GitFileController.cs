using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VGManager.Api.GitRepository.Request;
using VGManager.Api.GitRepository.Response;
using VGManager.Services.Interfaces;

namespace VGManager.Api.GitFile;

[Route("api/[controller]")]
[ApiController]
[EnableCors("_allowSpecificOrigins")]
public class GitFileController: ControllerBase
{
    private readonly IGitFileService _gitFileService;

    public GitFileController(IGitFileService gitFileService)
    {
        _gitFileService = gitFileService;
    }

    [HttpPost(Name = "files")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<GitFileResponse>> GetAsync(
        [FromBody] GitFileRequest request,
        CancellationToken cancellationToken
    )
    {
        (var status, var filePaths) = await _gitFileService.GetFilePathAsync(
            request.Organization,
            request.PAT,
            request.RepositoryId,
            request.FileName,
            request.Branch,
            cancellationToken
            );

        var result = new GitFileResponse
        {
            Status = status,
            FilePaths = filePaths
        };
        return Ok(result);
    }
}
