
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VGManager.Api.Secrets.Request;
using VGManager.Api.Secrets.Response;
using VGManager.Services.Interfaces;

namespace VGManager.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/[controller]")]
[ApiController]
public class SecretController : Controller
{
    private readonly IKeyVaultService _keyVaultService;
    private readonly IMapper _mapper;

    public SecretController(IKeyVaultService keyVaultService, IMapper mapper)
    {
        _keyVaultService = keyVaultService;
        _mapper = mapper;
    }

    [HttpGet(Name = "GetSecrets")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SecretsGetResponse>> GetAsync(
        [FromQuery] SecretGetRequest request,
        CancellationToken cancellationToken
        )
    {
        _keyVaultService.SetupConnectionRepository(request.KeyVaultName);
        var matchedSecrets = await _keyVaultService.GetSecretsAsync(request.SecretFilter, cancellationToken);

        var result = _mapper.Map<SecretsGetResponse>(matchedSecrets);
        return Ok(result);
    }

    [HttpGet("Deleted", Name = "GetDeletedSecrets")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<DeletedSecretsGetResponse> GetDeleted(
        [FromQuery] SecretGetRequest request,
        CancellationToken cancellationToken
        )
    {
        _keyVaultService.SetupConnectionRepository(request.KeyVaultName);
        var matchedDeletedSecrets = _keyVaultService.GetDeletedSecrets(request.SecretFilter, cancellationToken);

        var result = _mapper.Map<DeletedSecretsGetResponse>(matchedDeletedSecrets);
        return Ok(result);
    }

    [HttpPost("Delete", Name = "DeleteSecrets")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SecretsGetResponse>> DeleteAsync(
        [FromBody] SecretDeleteRequest request,
        CancellationToken cancellationToken
        )
    {
        _keyVaultService.SetupConnectionRepository(request.KeyVaultName);
        await _keyVaultService.DeleteAsync(request.SecretFilter, cancellationToken);
        var matchedSecrets = await _keyVaultService.GetSecretsAsync(request.SecretFilter, cancellationToken);

        var result = _mapper.Map<SecretsGetResponse>(matchedSecrets);
        return Ok(result);
    }

    [HttpPost("Recover", Name = "RecoverSecrets")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SecretsGetResponse>> RecoverAsync(
        [FromBody] SecretDeleteRequest request,
        CancellationToken cancellationToken
        )
    {
        _keyVaultService.SetupConnectionRepository(request.KeyVaultName);
        await _keyVaultService.RecoverSecretAsync(request.SecretFilter, cancellationToken);
        var matchedSecrets = _keyVaultService.GetDeletedSecrets(request.SecretFilter, cancellationToken);

        var result = _mapper.Map<SecretsGetResponse>(matchedSecrets);
        return Ok(result);
    }
}
