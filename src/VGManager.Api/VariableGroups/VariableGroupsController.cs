using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VGManager.Api.VariableGroups.Request;
using VGManager.Services.Interfaces;
using VGManager.Services.Models;
using VGManager.Services.Models.MatchedModels;

namespace VGManager.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/[controller]")]
[ApiController]
public class VariableGroupsController : ControllerBase
{
    private readonly IVariableGroupService _vgService;
    private readonly IMapper _mapper;

    public VariableGroupsController(
        IVariableGroupService vgService,
        IMapper mapper
        )
    {
        _vgService = vgService;
        _mapper = mapper;
    }

    [HttpGet(Name = "getvariablegroups")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<MatchedVariableGroup>>> GetAsync(
        [FromQuery] VariableGroupGetRequest request,
        CancellationToken cancellationToken
    )
    {
        var vgServiceModel = _mapper.Map<VariableGroupGetModel>(request);
        _vgService.SetupConnectionRepository(request.Organization, request.Project, request.PAT);

        var matchedVariableGroups = await _vgService.GetVariableGroupsAsync(
                request.VariableGroupFilter,
                request.KeyFilter,
                request.ValueFilter!,
                cancellationToken
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
        _vgService.SetupConnectionRepository(request.Organization, request.Project, request.PAT);

        await _vgService.UpdateVariableGroupsAsync(
                request.VariableGroupFilter, request.KeyFilter,
                request.NewValue, request.ValueFilter!, cancellationToken
            );

        var matchedVariableGroups = await _vgService.GetVariableGroupsAsync(
                request.VariableGroupFilter,
                request.KeyFilter,
                request.ValueFilter!, cancellationToken
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
        _vgService.SetupConnectionRepository(request.Organization, request.Project, request.PAT);

        await _vgService.AddVariableAsync(
                request.VariableGroupFilter, request.KeyFilter,
                request.Key,
                request.Value, cancellationToken
            );

        var matchedVariableGroups = await _vgService.GetVariableGroupsAsync(
                request.VariableGroupFilter,
                request.Key, null!, cancellationToken
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
        _vgService.SetupConnectionRepository(request.Organization, request.Project, request.PAT);

        await _vgService.DeleteVariableAsync(
                request.VariableGroupFilter,
                request.KeyFilter,
                request.ValueFilter!, cancellationToken
            );

        var matchedVariableGroups = await _vgService.GetVariableGroupsAsync(
                request.VariableGroupFilter,
                request.KeyFilter,
                request.ValueFilter!, cancellationToken
            );

        return Ok(matchedVariableGroups);
    }
}