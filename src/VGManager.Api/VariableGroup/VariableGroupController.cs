using AutoMapper;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<ActionResult<VariableGroupResponses>> GetAsync(
        [FromQuery] VariableGroupRequest request,
        CancellationToken cancellationToken
    )
    {
        VariableGroupResponses? result;
        if (request.Project == "All")
        {
            result = GetEmptyVariableGroupGetResponses();
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
        else
        {
            result = await GetResultAsync(request, cancellationToken);
        }
        return Ok(result);
    }

    [HttpPost("Update", Name = "UpdateVariableGroups")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<VariableGroupResponses>> UpdateAsync(
        [FromBody] VariableGroupUpdateRequest request,
        CancellationToken cancellationToken
    )
    {
        VariableGroupResponses? result;
        if (request.Project == "All")
        {
            result = GetEmptyVariableGroupGetResponses();
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
        else
        {
            result = await GetResultAsync(request, cancellationToken);
        }
        return Ok(result);
    }

    [HttpPost("UpdateInline", Name = "UpdateInline")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Status>> UpdateInlineAsync(
        [FromBody] VariableGroupUpdateRequest request,
        CancellationToken cancellationToken
    )
    {
        var vgServiceModel = _mapper.Map<VariableGroupUpdateModel>(request);

        _vgService.SetupConnectionRepository(vgServiceModel);
        var status = await _vgService.UpdateVariableGroupsAsync(vgServiceModel, cancellationToken);

        return Ok(status);
    }

    [HttpPost("Add", Name = "AddVariableGroups")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<VariableGroupResponses>> AddAsync(
        [FromBody] VariableGroupAddRequest request,
        CancellationToken cancellationToken
    )
    {
        VariableGroupResponses? result;
        if (request.Project == "All")
        {
            result = GetEmptyVariableGroupGetResponses();
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
        else
        {
            result = await GetResultAsync(request, cancellationToken);
        }
        return Ok(result);
    }

    [HttpPost("Delete", Name = "DeleteVariableGroups")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<VariableGroupResponses>> DeleteAsync(
        [FromBody] VariableGroupRequest request,
        CancellationToken cancellationToken
    )
    {
        VariableGroupResponses? result;
        if(request.Project == "All")
        {
            result = GetEmptyVariableGroupGetResponses();
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
        else
        {
            result = await GetResultAfterDeleteAsync(request, cancellationToken);
        }
        return Ok(result);
    }

    [HttpPost("DeleteInline", Name = "DeleteInline")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Status>> DeleteInlineAsync(
        [FromBody] VariableGroupRequest request,
        CancellationToken cancellationToken
    )
    {
        var vgServiceModel = _mapper.Map<VariableGroupModel>(request);

        _vgService.SetupConnectionRepository(vgServiceModel);
        var status = await _vgService.DeleteVariableAsync(vgServiceModel, cancellationToken);

        return Ok(status);
    }

    private static VariableGroupResponses GetEmptyVariableGroupGetResponses()
    {
        return new VariableGroupResponses
        {
            Status = Status.Success,
            VariableGroups = new List<VariableGroupResponse>()
        };
    }

    private async Task<ProjectResult> GetProjectsAsync(VariableGroupRequest request, CancellationToken cancellationToken)
    {
        var projectModel = new ProjectModel
        {
            Organization = request.Organization,
            PAT = request.PAT
        };

        var projectResponse = await _projectService.GetProjectsAsync(projectModel, cancellationToken);
        return projectResponse;
    }

    private async Task<VariableGroupResponses> GetResultAfterDeleteAsync(VariableGroupRequest request, CancellationToken cancellationToken)
    {
        var vgServiceModel = _mapper.Map<VariableGroupModel>(request);

        _vgService.SetupConnectionRepository(vgServiceModel);
        var status = await _vgService.DeleteVariableAsync(vgServiceModel, cancellationToken);
        var variableGroupResultModel = await _vgService.GetVariableGroupsAsync(vgServiceModel, cancellationToken);

        var result = _mapper.Map<VariableGroupResponses>(variableGroupResultModel);
        
        if(status != Status.Success)
        {
            result.Status = status;
        }

        return result;
    }

    private async Task<VariableGroupResponses> GetResultAsync(VariableGroupRequest request, CancellationToken cancellationToken)
    {
        var vgServiceModel = _mapper.Map<VariableGroupModel>(request);

        _vgService.SetupConnectionRepository(vgServiceModel);
        var variableGroupResultsModel = await _vgService.GetVariableGroupsAsync(vgServiceModel, cancellationToken);

        var result = _mapper.Map<VariableGroupResponses>(variableGroupResultsModel);
        return result;
    }

    private async Task<VariableGroupResponses> GetResultAsync(VariableGroupAddRequest request, CancellationToken cancellationToken)
    {
        var vgServiceModel = _mapper.Map<VariableGroupAddModel>(request);

        _vgService.SetupConnectionRepository(vgServiceModel);
        var status = await _vgService.AddVariablesAsync(vgServiceModel, cancellationToken);
        vgServiceModel.KeyFilter = vgServiceModel.Key;
        vgServiceModel.ValueFilter = vgServiceModel.Value;
        var variableGroupResultModel = await _vgService.GetVariableGroupsAsync(vgServiceModel, cancellationToken);

        var result = _mapper.Map<VariableGroupResponses>(variableGroupResultModel);

        if (status != Status.Success)
        {
            result.Status = status;
        }

        return result;
    }

    private async Task<VariableGroupResponses> GetResultAsync(VariableGroupUpdateRequest request, CancellationToken cancellationToken)
    {
        var vgServiceModel = _mapper.Map<VariableGroupUpdateModel>(request);

        _vgService.SetupConnectionRepository(vgServiceModel);
        var status = await _vgService.UpdateVariableGroupsAsync(vgServiceModel, cancellationToken);

        vgServiceModel.ValueFilter = vgServiceModel.NewValue;
        var variableGroupResultModel = await _vgService.GetVariableGroupsAsync(vgServiceModel, cancellationToken);

        var result = _mapper.Map<VariableGroupResponses>(variableGroupResultModel);

        if (status != Status.Success)
        {
            result.Status = status;
        }

        return result;
    }
}
