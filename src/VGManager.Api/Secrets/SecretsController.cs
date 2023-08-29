
using Microsoft.AspNetCore.Mvc;
using VGManager.Api.Secrets.Request;
using VGManager.Services.Interfaces;
using VGManager.Services.Model;

namespace VGApi.Api.Secrets;

[ApiVersion("1.0")]
[Route("api/[controller]")]
[ApiController]
public class SecretsController : Controller
{
    private readonly IKVService _kvService;

    public SecretsController(IKVService kvService)
    {
        _kvService = kvService;
    }

    [HttpGet("getsecrets", Name = "getsecrets")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<MatchedSecret>>> GetAsync(
        string keyVaultName,
        string secretFilter,
        CancellationToken cancellationToken)
    {
        _kvService.SetupConnectionRepository(keyVaultName);
        var matchedSecrets = await _kvService.GetSecretsAsync(secretFilter);
        return Ok(matchedSecrets);
    }

    [HttpGet("getdeletedsecrets", Name = "getdeletedsecrets")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<MatchedDeletedSecret>>> GetDeletedAsync(
        string keyVaultName,
        string secretFilter,
        CancellationToken cancellationToken
    )
    {
        _kvService.SetupConnectionRepository(keyVaultName);
        var matchedSecrets = await _kvService.GetDeletedSecretsAsync(secretFilter);
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
        await _kvService.DeleteAsync(request.SecretFilter);
        var matchedSecrets = await _kvService.GetSecretsAsync(request.SecretFilter);
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
        await _kvService.RecoverSecretAsync(request.SecretFilter);
        var matchedSecrets = await _kvService.GetDeletedSecretsAsync(request.SecretFilter);
        return Ok(matchedSecrets);
    }
}
