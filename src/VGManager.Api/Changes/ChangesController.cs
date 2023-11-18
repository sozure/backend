using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VGManager.Api.Changes.Request;
using VGManager.Api.Changes.Response;
using VGManager.Entities;
using VGManager.Services.Interfaces;
using VGManager.Services.Models.Changes;

namespace VGManager.Api.Changes;

[Route("api/[controller]")]
[ApiController]
[EnableCors("_allowSpecificOrigins")]
public class ChangesController : ControllerBase
{

    private readonly IChangesService _changesService;

    public ChangesController(IChangesService changesService)
    {
        _changesService = changesService;
    }

    [HttpPost("Get", Name = "getchanges")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ChangesResponse>> GetAsync(
        [FromBody] ChangesRequest request,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var result = await _changesService.GetAsync(request.Limit, request.ChangeTypes, cancellationToken);
            return Ok(new ChangesResponse
            {
                Status = RepositoryStatus.Success,
                Operations = result
            });
        }
        catch (Exception)
        {
            return Ok(new ChangesResponse
            {
                Status = RepositoryStatus.Unknown,
                Operations = Array.Empty<OperationModel>()
            });
        }
    }
}
