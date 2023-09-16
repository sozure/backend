using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using VGManager.Api.VariableGroup.Response;
using VGManager.Api.VariableGroups.Request;
using VGManager.Api.VariableGroups.Response;
using VGManager.AzureAdapter.Entities;
using VGManager.Services.Interfaces;
using VGManager.Services.Models.Projects;
using VGManager.Services.Models.VariableGroups.Requests;

namespace VGManager.Api.Controllers;

[ApiVersion("1.0")]
[Route("api/[controller]")]
[ApiController]
public class VariableGroupController : ControllerBase
{
    private readonly IVariableGroupService _vgService;
    private readonly IProjectService _projectService;
    private readonly IMapper _mapper;

    public VariableGroupController(
        IVariableGroupService vgService,
        IProjectService projectService,
        IMapper mapper
        )
    {
        _vgService = vgService;
        _projectService = projectService;
        _mapper = mapper;
    }

    [HttpGet(Name = "GetVariableGroups")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<VariableGroupGetResponses>> GetAsync(
        [FromQuery] VariableGroupRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await GetResultAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("GetFromMultipleProjects", Name = "GetVariableGroupsFromMultipleProjects")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<VariableGroupGetResponses>> GetFromMultipleProjectsAsync(
        [FromQuery] VariableGroupRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = GetEmptyVariableGroupGetResponses();

        if (request.Project == "All")
        {
            var projectResponse = await GetProjectsAsync(request, cancellationToken);

            foreach (var project in projectResponse.Projects)
            {
                request.Project = project.Name;
                var subResult = await GetResultAsync(request, cancellationToken);
                result.VariableGroups.AddRange(subResult.VariableGroups);

                if (subResult.Status != Status.Success)
                {
                    result.Status = subResult.Status;
                }
            }
        }

        return Ok(result);
    }

    [HttpPost("Update", Name = "UpdateVariableGroups")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<VariableGroupGetResponses>> UpdateAsync(
        [FromBody] VariableGroupUpdateRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await GetResultAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("UpdateFromMultipleProjects", Name = "UpdateVariableGroupsFromMultipleProjects")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<VariableGroupGetResponses>> UpdateFromMultipleProjectsAsync(
        [FromBody] VariableGroupUpdateRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = GetEmptyVariableGroupGetResponses();

        if (request.Project == "All")
        {
            var projectResponse = await GetProjectsAsync(request, cancellationToken);

            foreach (var project in projectResponse.Projects)
            {
                request.Project = project.Name;
                var subResult = await GetResultAsync(request, cancellationToken);
                result.VariableGroups.AddRange(subResult.VariableGroups);

                if (subResult.Status != Status.Success)
                {
                    result.Status = subResult.Status;
                }
            }
        }

        return Ok(result);
    }

    [HttpPost("Add", Name = "AddVariableGroups")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<VariableGroupGetResponses>> AddAsync(
        [FromBody] VariableGroupAddRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await GetResultAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("AddFromMultipleProjects", Name = "AddVariableGroupsFromMultipleProjects")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<VariableGroupGetResponses>> AddFromMultipleProjectsAsync(
        [FromBody] VariableGroupAddRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = GetEmptyVariableGroupGetResponses();

        if (request.Project == "All")
        {
            var projectResponse = await GetProjectsAsync(request, cancellationToken);

            foreach (var project in projectResponse.Projects)
            {
                request.Project = project.Name;
                var subResult = await GetResultAsync(request, cancellationToken);
                result.VariableGroups.AddRange(subResult.VariableGroups);

                if (subResult.Status != Status.Success)
                {
                    result.Status = subResult.Status;
                }
            }
        }

        return Ok(result);
    }

    [HttpPost("Delete", Name = "DeleteVariableGroups")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<VariableGroupGetResponses>> DeleteAsync(
        [FromBody] VariableGroupRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await GetResultAfterDeleteAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("DeleteFromMultipleProjects", Name = "DeleteVariableGroupsFromMultipleProjects")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<VariableGroupGetResponses>> DeleteFromMultipleProjectsAsync(
        [FromBody] VariableGroupRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = GetEmptyVariableGroupGetResponses();

        if (request.Project == "All")
        {
            var projectResponse = await GetProjectsAsync(request, cancellationToken);

            foreach (var project in projectResponse.Projects)
            {
                request.Project = project.Name;
                var subResult = await GetResultAfterDeleteAsync(request, cancellationToken);
                result.VariableGroups.AddRange(subResult.VariableGroups);

                if (subResult.Status != Status.Success)
                {
                    result.Status = subResult.Status;
                }
            }
        }

        return Ok(result);
    }

    private static VariableGroupGetResponses GetEmptyVariableGroupGetResponses()
    {
        return new VariableGroupGetResponses
        {
            Status = Status.Success,
            VariableGroups = new List<VariableGroupGetBaseResponse>()
        };
    }

    private async Task<ProjectResultModel> GetProjectsAsync(VariableGroupRequest request, CancellationToken cancellationToken)
    {
        var projectModel = new ProjectModel
        {
            Organization = request.Organization,
            PAT = request.PAT
        };

        var projectResponse = await _projectService.GetProjectsAsync(projectModel, cancellationToken);
        return projectResponse;
    }

    private async Task<VariableGroupGetResponses> GetResultAfterDeleteAsync(VariableGroupRequest request, CancellationToken cancellationToken)
    {
        var vgServiceModel = _mapper.Map<VariableGroupModel>(request);

        _vgService.SetupConnectionRepository(vgServiceModel);
        await _vgService.DeleteVariableAsync(vgServiceModel, cancellationToken);
        var variableGroupResultModel = await _vgService.GetVariableGroupsAsync(vgServiceModel, cancellationToken);

        var result = _mapper.Map<VariableGroupGetResponses>(variableGroupResultModel);
        return result;
    }

    private async Task<VariableGroupGetResponses> GetResultAsync(VariableGroupRequest request, CancellationToken cancellationToken)
    {
        var vgServiceModel = _mapper.Map<VariableGroupModel>(request);

        _vgService.SetupConnectionRepository(vgServiceModel);
        var variableGroupResultsModel = await _vgService.GetVariableGroupsAsync(vgServiceModel, cancellationToken);

        var result = _mapper.Map<VariableGroupGetResponses>(variableGroupResultsModel);
        return result;
    }

    private async Task<VariableGroupGetResponses> GetResultAsync(VariableGroupAddRequest request, CancellationToken cancellationToken)
    {
        var vgServiceModel = _mapper.Map<VariableGroupAddModel>(request);

        _vgService.SetupConnectionRepository(vgServiceModel);
        await _vgService.AddVariablesAsync(vgServiceModel, cancellationToken);
        vgServiceModel.KeyFilter = vgServiceModel.Key;
        vgServiceModel.ValueFilter = vgServiceModel.Value;
        var variableGroupResultModel = await _vgService.GetVariableGroupsAsync(vgServiceModel, cancellationToken);

        var result = _mapper.Map<VariableGroupGetResponses>(variableGroupResultModel);
        return result;
    }

    private async Task<VariableGroupGetResponses> GetResultAsync(VariableGroupUpdateRequest request, CancellationToken cancellationToken)
    {
        var vgServiceModel = _mapper.Map<VariableGroupUpdateModel>(request);

        _vgService.SetupConnectionRepository(vgServiceModel);
        await _vgService.UpdateVariableGroupsAsync(vgServiceModel, cancellationToken);

        vgServiceModel.ValueFilter = vgServiceModel.NewValue;
        var variableGroupResultModel = await _vgService.GetVariableGroupsAsync(vgServiceModel, cancellationToken);

        var result = _mapper.Map<VariableGroupGetResponses>(variableGroupResultModel);
        return result;
    }
}
