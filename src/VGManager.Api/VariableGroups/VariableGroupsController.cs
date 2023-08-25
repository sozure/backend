using Microsoft.AspNetCore.Mvc;
using VGManager.Api.VariableGroups.Request;
using VGManager.Services.Interfaces;
using VGManager.Services.Model;
using VGManager.Services.Repositories.Interface;

namespace VGManager.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/[controller]")]
[ApiController]
public class VariableGroupsController : ControllerBase
{
    private readonly IVariableGroupConnectionRepository _variableGroupConnectionRepository;
    private readonly IVariableGroupService _vgService;

    public VariableGroupsController(IVariableGroupService vgService, IVariableGroupConnectionRepository variableGroupConnectionRepository)
    {
        _vgService = vgService;
        _variableGroupConnectionRepository = variableGroupConnectionRepository;
    }

    [HttpGet("getvariablegroups", Name = "getvariablegroups")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<MatchedVariableGroup>>> GetAsync(
        string organization,
        string project,
        string pat,
        string variableGroupFilter,
        string keyFilter,
        string? valueFilter,
        CancellationToken cancellationToken
    )
    {
        _variableGroupConnectionRepository.Setup(organization, project, pat);

        var matchedVariableGroups = await _vgService.GetVariableGroupsAsync(
                _variableGroupConnectionRepository,
                variableGroupFilter,
                keyFilter,
                valueFilter
            );

        return Ok(matchedVariableGroups);
    }

    [HttpPost("updatevariablegroups", Name = "updatevariablegroups")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<MatchedVariableGroup>>> UpdateAsync(
        [FromBody] VariableGroupUpdateRequest request,
        CancellationToken cancellationToken
    )
    {
        _variableGroupConnectionRepository.Setup(request.Organization, request.Project, request.Pat);

        await _vgService.UpdateVariableGroupsAsync(
                _variableGroupConnectionRepository,
                request.VariableGroupFilter, request.KeyFilter,
                request.NewValue, request.ValueFilter
            );

        var matchedVariableGroups = await _vgService.GetVariableGroupsAsync(
                _variableGroupConnectionRepository,
                request.VariableGroupFilter,
                request.KeyFilter,
                request.ValueFilter
            );

        return Ok(matchedVariableGroups);
    }

    [HttpPost("addvariable", Name = "addvariable")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<MatchedVariableGroup>>> AddAsync(
        [FromBody] VariableGroupAddRequest request,
        CancellationToken cancellationToken
    )
    {
        _variableGroupConnectionRepository.Setup(request.Organization, request.Project, request.Pat);

        await _vgService.AddVariableAsync(
                _variableGroupConnectionRepository,
                request.VariableGroupFilter, request.KeyFilter,
                request.Key,
                request.Value
            );

        var matchedVariableGroups = await _vgService.GetVariableGroupsAsync(
                _variableGroupConnectionRepository,
                request.VariableGroupFilter,
                request.Key, null!
            );

        return Ok(matchedVariableGroups);
    }

    [HttpPost("deletevariable", Name = "deletevariable")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<MatchedVariableGroup>>> DeleteAsync(
        [FromBody] VariableGroupDeleteRequest request,
        CancellationToken cancellationToken
    )
    {
        _variableGroupConnectionRepository.Setup(request.Organization, request.Project, request.Pat);

        await _vgService.DeleteVariableAsync(
            _variableGroupConnectionRepository,
                request.VariableGroupFilter,
                request.KeyFilter,
                request.ValueFilter
            );

        var matchedVariableGroups = await _vgService.GetVariableGroupsAsync(
                _variableGroupConnectionRepository,
                request.VariableGroupFilter,
                request.KeyFilter,
                request.ValueFilter
            );

        return Ok(matchedVariableGroups);
    }
}