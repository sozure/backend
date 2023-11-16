using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VGManager.Api.Changes.Request;
using VGManager.Api.Changes.Response;
using VGManager.Entities;
using VGManager.Services.Interfaces;

namespace VGManager.Api.Changes;

[Route("api/[controller]")]
[ApiController]
[EnableCors("_allowSpecificOrigins")]
public class ChangesController: ControllerBase
{

    private readonly IChangesService _changesService;

    public ChangesController(IChangesService changesService)
    {
        _changesService = changesService;
    }

    [HttpPost("GetByDate", Name = "GetByDate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ChangesResponse>> GetByDateAsync(
        [FromBody] ChangesRequestByDate request, 
        CancellationToken cancellationToken
        )
    {
        var result = _changesService.GetByDateAsync(request.From, request.To, request.ChangeTypes, cancellationToken);
        return Ok(new ChangesResponse
        {
            Status = RepositoryStatus.Success
        });
    }

    [HttpPost("GetByMaxLimit", Name = "GetByMaxLimit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ChangesResponse>> GetByMaxLimitAsync(
        [FromBody] ChangesRequestByLimit request,
        CancellationToken cancellationToken
        )
    {
        return Ok();
    }
}
