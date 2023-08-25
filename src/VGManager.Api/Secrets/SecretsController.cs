
using Microsoft.AspNetCore.Mvc;
using System.Web.Http.Cors;
using VGManager.Api.Secrets.Request;
using VGManager.Services.Interfaces;
using VGManager.Services.Model;
using VGManager.Services.Repositories.Interfaces;

namespace VGApi.Api.Secrets;

[ApiVersion("1.0")]
[Route("api/[controller]")]
[ApiController]
[EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
public class SecretsController : Controller
{
    private readonly IKeyVaultConnectionRepository _keyVaultConnectionRepository;
    private readonly IKVService _kvService;

    public SecretsController(IKeyVaultConnectionRepository keyVaultConnectionRepository, IKVService kvService)
    {
        _keyVaultConnectionRepository = keyVaultConnectionRepository;
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
        _keyVaultConnectionRepository.Setup(keyVaultName);
        var matchedSecrets = await _kvService.GetSecretsAsync(_keyVaultConnectionRepository, secretFilter);
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
        _keyVaultConnectionRepository.Setup(keyVaultName);
        var matchedSecrets = await _kvService.GetDeletedSecretsAsync(_keyVaultConnectionRepository, secretFilter);
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
        _keyVaultConnectionRepository.Setup(request.KeyVaultName);
        await _kvService.DeleteAsync(_keyVaultConnectionRepository, request.SecretFilter);
        var matchedSecrets = await _kvService.GetSecretsAsync(_keyVaultConnectionRepository, request.SecretFilter);
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
        _keyVaultConnectionRepository.Setup(request.KeyVaultName);
        await _kvService.RecoverSecretAsync(_keyVaultConnectionRepository, request.SecretFilter);
        var matchedSecrets = await _kvService.GetDeletedSecretsAsync(_keyVaultConnectionRepository, request.SecretFilter);
        return Ok(matchedSecrets);
    }
}
