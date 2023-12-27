using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using VGManager.Api.Changes.Request;
using VGManager.Api.Changes.Response;
using VGManager.Entities;
using VGManager.Services.Interfaces;
using VGManager.Services.Models.Changes.Requests;
using VGManager.Services.Models.Changes.Responses;

namespace VGManager.Api.Changes;

[Route("api/[controller]")]
[ApiController]
[EnableCors("_allowSpecificOrigins")]
public class ChangesController : ControllerBase
{

    private readonly IChangeService _changesService;
    private readonly IMapper _mapper;

    public ChangesController(IChangeService changesService, IMapper mapper)
    {
        _changesService = changesService;
        _mapper = mapper;
    }

    [HttpPost("Variables", Name = "getvariablechanges")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<VGChangesResponse>> GetVariableChangesAsync(
        [FromBody] VGChangesRequest request,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var result = await _changesService.GetAsync(_mapper.Map<VGRequestModel>(request), cancellationToken);
            return Ok(new VGChangesResponse
            {
                Status = RepositoryStatus.Success,
                Operations = result
            });
        }
        catch (Exception)
        {
            return Ok(new VGChangesResponse
            {
                Status = RepositoryStatus.Unknown,
                Operations = Array.Empty<VGOperationModel>()
            });
        }
    }

    [HttpPost("Secrets", Name = "getsecretchanges")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SecretChangesResponse>> GetSecretChangesAsync(
        [FromBody] SecretChangesRequest request,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var result = await _changesService.GetAsync(_mapper.Map<SecretRequestModel>(request), cancellationToken);
            return Ok(new SecretChangesResponse
            {
                Status = RepositoryStatus.Success,
                Operations = result
            });
        }
        catch (Exception)
        {
            return Ok(new SecretChangesResponse
            {
                Status = RepositoryStatus.Unknown,
                Operations = Array.Empty<SecretOperationModel>()
            });
        }
    }

    [HttpPost("KeyVaultCopies", Name = "getkvchanges")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<KVChangesResponse>> GetKVChangesAsync(
        [FromBody] KVChangesRequest request,
        CancellationToken cancellationToken
        )
    {
        try
        {
            var result = await _changesService.GetAsync(_mapper.Map<KVRequestModel>(request), cancellationToken);
            return Ok(new KVChangesResponse
            {
                Status = RepositoryStatus.Success,
                Operations = result
            });
        }
        catch (Exception)
        {
            return Ok(new KVChangesResponse
            {
                Status = RepositoryStatus.Unknown,
                Operations = Array.Empty<KVOperationModel>()
            });
        }
    }
}
