
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VGManager.Api.Secrets.Request;
using VGManager.Api.Secrets.Response;
using VGManager.Services.Interfaces;
using VGManager.Services.Models.MatchedModels;

namespace VGApi.Api.Secrets;

[ApiVersion("1.0")]
[Route("api/[controller]")]
[ApiController]
public class SecretsController : Controller
{
    private readonly IKeyVaultService _kvService;
    private readonly IMapper _mapper;

    public SecretsController(IKeyVaultService kvService, IMapper mapper)
    {
        _kvService = kvService;
        _mapper = mapper;
    }

    [HttpGet("getsecrets", Name = "getsecrets")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<SecretGetResponse>>> GetAsync(
        [FromQuery] SecretGetRequest request,
        CancellationToken cancellationToken
        )
    {
        _kvService.SetupConnectionRepository(request.KeyVaultName);
        var matchedSecrets = await _kvService.GetSecretsAsync(request.SecretFilter, cancellationToken);

        var result = _mapper.Map<SecretGetResponse>(matchedSecrets);
        return Ok(result);
    }

    [HttpGet("getdeletedsecrets", Name = "getdeletedsecrets")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<DeletedSecretGetResponse>> GetDeleted(
        [FromQuery] SecretGetRequest request,
        CancellationToken cancellationToken
        )
    {
        _kvService.SetupConnectionRepository(request.KeyVaultName);
        var matchedDeletedSecrets = _kvService.GetDeletedSecrets(request.SecretFilter, cancellationToken);

        var result = _mapper.Map<DeletedSecretGetResponse>(matchedDeletedSecrets);
        return Ok(result);
    }

    [HttpPost("deletesecret", Name = "deletesecret")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<SecretGetResponse>>> DeleteAsync(
        [FromBody] SecretDeleteRequest request,
        CancellationToken cancellationToken
        )
    {
        _kvService.SetupConnectionRepository(request.KeyVaultName);
        await _kvService.DeleteAsync(request.SecretFilter, cancellationToken);
        var matchedSecrets = await _kvService.GetSecretsAsync(request.SecretFilter, cancellationToken);

        var result = _mapper.Map<SecretGetResponse>(matchedSecrets);
        return Ok(result);
    }

    [HttpPost("recoversecret", Name = "recoversecret")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<SecretGetResponse>>> RecoverAsync(
        [FromBody] SecretDeleteRequest request,
        CancellationToken cancellationToken
        )
    {
        _kvService.SetupConnectionRepository(request.KeyVaultName);
        await _kvService.RecoverSecretAsync(request.SecretFilter, cancellationToken);
        var matchedSecrets = _kvService.GetDeletedSecrets(request.SecretFilter, cancellationToken);

        var result = _mapper.Map<SecretGetResponse>(matchedSecrets);
        return Ok(result);
    }
}
