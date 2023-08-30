
using Microsoft.AspNetCore.Mvc;
using VGManager.Api.Secrets.Request;
using VGManager.Services.Interfaces;
using VGManager.Services.Models;

namespace VGApi.Api.Secrets;

[ApiVersion("1.0")]
[Route("api/[controller]")]
[ApiController]
public class SecretsController : Controller
{
    private readonly IKeyVaultService _kvService;

    public SecretsController(IKeyVaultService kvService)
    {
        _kvService = kvService;
    }

    [HttpGet("getsecrets", Name = "getsecrets")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<MatchedSecret>>> GetAsync(
        [FromQuery] SecretGetRequest request,
        CancellationToken cancellationToken
        )
    {
        _kvService.SetupConnectionRepository(request.KeyVaultName);
        var matchedSecrets = await _kvService.GetSecretsAsync(request.SecretFilter, cancellationToken);
        return Ok(matchedSecrets);
    }

    [HttpGet("getdeletedsecrets", Name = "getdeletedsecrets")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<MatchedDeletedSecret>> GetDeleted(
        [FromQuery] SecretGetRequest request,
        CancellationToken cancellationToken
        )
    {
        _kvService.SetupConnectionRepository(request.KeyVaultName);
        var matchedSecrets = _kvService.GetDeletedSecrets(request.SecretFilter, cancellationToken);
        return Ok(matchedSecrets);
    }

    [HttpPost("deletesecret", Name = "deletesecret")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<MatchedSecret>>> DeleteAsync(
        [FromBody] SecretDeleteRequest request,
        CancellationToken cancellationToken
        )
    {
        _kvService.SetupConnectionRepository(request.KeyVaultName);
        await _kvService.DeleteAsync(request.SecretFilter, cancellationToken);
        var matchedSecrets = await _kvService.GetSecretsAsync(request.SecretFilter, cancellationToken);
        return Ok(matchedSecrets);
    }

    [HttpPost("recoversecret", Name = "recoversecret")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<MatchedDeletedSecret>>> RecoverAsync(
        [FromBody] SecretDeleteRequest request,
        CancellationToken cancellationToken
        )
    {
        _kvService.SetupConnectionRepository(request.KeyVaultName);
        await _kvService.RecoverSecretAsync(request.SecretFilter, cancellationToken);
        var matchedSecrets = _kvService.GetDeletedSecrets(request.SecretFilter, cancellationToken);
        return Ok(matchedSecrets);
    }
}
