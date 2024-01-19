using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VGManager.Adapter.Models.Models;
using VGManager.Services.Interfaces;

namespace VGManager.Api.Endpoints.GitFile;

[Route("api/[controller]")]
[ApiController]
[EnableCors("_allowSpecificOrigins")]
public class GitFileController : ControllerBase
{
    private readonly IGitFileService _gitFileService;

    public GitFileController(IGitFileService gitFileService)
    {
        _gitFileService = gitFileService;
    }

    [HttpPost("FilePath", Name = "path")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AdapterResponseModel<IEnumerable<string>>>> GetFilePathAsync(
        [FromBody] GitFilePathRequest request,
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

        var result = new AdapterResponseModel<IEnumerable<string>>
        {
            Status = status,
            Data = filePaths
        };
        return Ok(result);
    }

    [HttpPost("ConfigFiles", Name = "config")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AdapterResponseModel<IEnumerable<string>>>> GetConfigFilesAsync(
        [FromBody] GitConfigFileRequest request,
        CancellationToken cancellationToken
        )
    {
        (var status, var configFiles) = await _gitFileService.GetConfigFilesAsync(
            request.Organization,
            request.PAT,
            request.RepositoryId,
            request.Extension,
            request.Branch,
            cancellationToken
            );

        var result = new AdapterResponseModel<IEnumerable<string>>
        {
            Status = status,
            Data = configFiles
        };

        return Ok(result);
    }
}
