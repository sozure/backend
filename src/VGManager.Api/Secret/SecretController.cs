
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VGManager.Api.Secret.Request;
using VGManager.Api.Secrets.Response;
using VGManager.AzureAdapter.Entities;
using VGManager.Services.Interfaces;
using VGManager.Services.Models.Secrets;

namespace VGManager.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/[controller]")]
[ApiController]
public class SecretController : ControllerBase
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
        [FromQuery] SecretRequest request,
        CancellationToken cancellationToken
        )
    {
        var secretModel = _mapper.Map<SecretModel>(request);

        _keyVaultService.SetupConnectionRepository(secretModel);
        var matchedSecrets = await _keyVaultService.GetSecretsAsync(secretModel.SecretFilter, cancellationToken);

        var result = _mapper.Map<SecretsGetResponse>(matchedSecrets);
        return Ok(result);
    }

    [HttpGet("Deleted", Name = "GetDeletedSecrets")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<DeletedSecretsGetResponse> GetDeleted(
        [FromQuery] SecretRequest request,
        CancellationToken cancellationToken
        )
    {
        var secretModel = _mapper.Map<SecretModel>(request);

        _keyVaultService.SetupConnectionRepository(secretModel);
        var matchedDeletedSecrets = _keyVaultService.GetDeletedSecrets(secretModel.SecretFilter, cancellationToken);

        var result = _mapper.Map<DeletedSecretsGetResponse>(matchedDeletedSecrets);
        return Ok(result);
    }

    [HttpPost("Delete", Name = "DeleteSecrets")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SecretsGetResponse>> DeleteAsync(
        [FromBody] SecretRequest request,
        CancellationToken cancellationToken
        )
    {
        var secretModel = _mapper.Map<SecretModel>(request);

        _keyVaultService.SetupConnectionRepository(secretModel);
        await _keyVaultService.DeleteAsync(secretModel.SecretFilter, cancellationToken);
        var matchedSecrets = await _keyVaultService.GetSecretsAsync(secretModel.SecretFilter, cancellationToken);

        var result = _mapper.Map<SecretsGetResponse>(matchedSecrets);
        return Ok(result);
    }

    [HttpPost("Recover", Name = "RecoverSecrets")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SecretsGetResponse>> RecoverAsync(
        [FromBody] SecretRequest request,
        CancellationToken cancellationToken
        )
    {
        var secretModel = _mapper.Map<SecretModel>(request);

        _keyVaultService.SetupConnectionRepository(secretModel);
        await _keyVaultService.RecoverSecretAsync(secretModel.SecretFilter, cancellationToken);
        var matchedSecrets = _keyVaultService.GetDeletedSecrets(secretModel.SecretFilter, cancellationToken);

        var result = _mapper.Map<SecretsGetResponse>(matchedSecrets);
        return Ok(result);
    }

    [HttpPost("Copy", Name = "CopySecrets")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Status>> CopyAsync(
        [FromBody] SecretCopyRequest request,
        CancellationToken cancellationToken
        )
    {
        var secretModel = _mapper.Map<SecretCopyModel>(request);
        var status = await _keyVaultService.CopySecretsAsync(secretModel, cancellationToken);
        return Ok(status);
    }
}
