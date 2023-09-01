using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VGManager.Api.VariableGroups.Request;
using VGManager.Api.VariableGroups.Response;
using VGManager.Services.Interfaces;
using VGManager.Services.Models.VariableGroups;

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
    public async Task<ActionResult<IEnumerable<VariableGroupGetResponse>>> GetAsync(
        [FromQuery] VariableGroupGetRequest request,
        CancellationToken cancellationToken
    )
    {
        var vgServiceModel = _mapper.Map<VariableGroupGetModel>(request);

        _vgService.SetupConnectionRepository(vgServiceModel);
        var matchedVariableGroups = await _vgService.GetVariableGroupsAsync(vgServiceModel, cancellationToken);

        var result = matchedVariableGroups.Select(_mapper.Map<VariableGroupGetResponse>);
        return Ok(result);
    }

    [HttpPost("updatevariablegroups", Name = "updatevariablegroups")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<VariableGroupGetResponse>>> UpdateAsync(
        [FromBody] VariableGroupUpdateRequest request,
        CancellationToken cancellationToken
    )
    {
        var vgServiceModel = _mapper.Map<VariableGroupUpdateModel>(request);

        _vgService.SetupConnectionRepository(vgServiceModel);
        await _vgService.UpdateVariableGroupsAsync(vgServiceModel, cancellationToken);
        vgServiceModel.ValueFilter = vgServiceModel.NewValue;
        var matchedVariableGroups = await _vgService.GetVariableGroupsAsync(vgServiceModel, cancellationToken);

        var result = matchedVariableGroups.Select(_mapper.Map<VariableGroupGetResponse>);
        return Ok(result);
    }

    [HttpPost("addvariable", Name = "addvariable")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<VariableGroupGetResponse>>> AddAsync(
        [FromBody] VariableGroupAddRequest request,
        CancellationToken cancellationToken
    )
    {
        var vgServiceModel = _mapper.Map<VariableGroupAddModel>(request);

        _vgService.SetupConnectionRepository(vgServiceModel);
        await _vgService.AddVariableAsync(vgServiceModel, cancellationToken);
        vgServiceModel.KeyFilter = vgServiceModel.Key;
        vgServiceModel.ValueFilter = vgServiceModel.Value;
        var matchedVariableGroups = await _vgService.GetVariableGroupsAsync(vgServiceModel, cancellationToken);

        var result = matchedVariableGroups.Select(_mapper.Map<VariableGroupGetResponse>);
        return Ok(result);
    }

    [HttpPost("deletevariable", Name = "deletevariable")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<VariableGroupGetResponse>>> DeleteAsync(
        [FromBody] VariableGroupDeleteRequest request,
        CancellationToken cancellationToken
    )
    {
        var vgServiceModel = _mapper.Map<VariableGroupDeleteModel>(request);

        _vgService.SetupConnectionRepository(vgServiceModel);
        await _vgService.DeleteVariableAsync(vgServiceModel, cancellationToken);
        var matchedVariableGroups = await _vgService.GetVariableGroupsAsync(vgServiceModel, cancellationToken);

        var result = matchedVariableGroups.Select(_mapper.Map<VariableGroupGetResponse>);
        return Ok(result);
    }
}