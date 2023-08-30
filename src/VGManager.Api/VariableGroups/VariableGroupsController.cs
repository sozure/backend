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

        _vgService.SetupConnectionRepository(vgServiceModel);
        var matchedVariableGroups = await _vgService.GetVariableGroupsAsync(vgServiceModel, cancellationToken);

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
        var vgServiceModel = _mapper.Map<VariableGroupUpdateModel>(request);

        _vgService.SetupConnectionRepository(vgServiceModel);
        await _vgService.UpdateVariableGroupsAsync(vgServiceModel, cancellationToken);
        var matchedVariableGroups = await _vgService.GetVariableGroupsAsync(vgServiceModel, cancellationToken);

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
        var vgServiceModel = _mapper.Map<VariableGroupAddModel>(request);

        _vgService.SetupConnectionRepository(vgServiceModel);
        await _vgService.AddVariableAsync(vgServiceModel, cancellationToken);
        var matchedVariableGroups = await _vgService.GetVariableGroupsAsync(vgServiceModel, cancellationToken);

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
        var vgServiceModel = _mapper.Map<VariableGroupDeleteModel>(request);

        _vgService.SetupConnectionRepository(vgServiceModel);
        await _vgService.DeleteVariableAsync(vgServiceModel, cancellationToken);
        var matchedVariableGroups = await _vgService.GetVariableGroupsAsync(vgServiceModel, cancellationToken);

        return Ok(matchedVariableGroups);
    }
}