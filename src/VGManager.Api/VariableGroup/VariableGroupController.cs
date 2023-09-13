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
    public async Task<ActionResult<VariableGroupGetResponses>> GetAsync(
        [FromQuery] VariableGroupGetRequest request,
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
        [FromQuery] VariableGroupGetRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = GetVariableGroupGetResponses();

        if (request.Project == "All")
        {
            var projectResponse = await GetProjectsAsync(request, cancellationToken);

            foreach (var project in projectResponse.Projects)
            {
                var singleRequest = GetSingleRequest(request, project.Name);
                var subResult = await GetResultAsync(singleRequest, cancellationToken);
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
        var result = GetVariableGroupGetResponses();

        if (request.Project == "All")
        {
            var projectResponse = await GetProjectsAsync(request, cancellationToken);

            foreach (var project in projectResponse.Projects)
            {
                var singleRequest = GetSingleRequest(request, project.Name);
                var subResult = await GetResultAsync(singleRequest, cancellationToken);
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
        var result = GetVariableGroupGetResponses();

        if (request.Project == "All")
        {
            var projectResponse = await GetProjectsAsync(request, cancellationToken);

            foreach (var project in projectResponse.Projects)
            {
                var singleRequest = GetSingleRequest(request, project.Name);
                var subResult = await GetResultAsync(singleRequest, cancellationToken);
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
        [FromBody] VariableGroupDeleteRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await GetResultAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("DeleteFromMultipleProjects", Name = "DeleteVariableGroupsFromMultipleProjects")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<VariableGroupGetResponses>> DeleteFromMultipleProjectsAsync(
        [FromBody] VariableGroupDeleteRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = GetVariableGroupGetResponses();

        if (request.Project == "All")
        {
            var projectResponse = await GetProjectsAsync(request, cancellationToken);

            foreach (var project in projectResponse.Projects)
            {
                var singleRequest = GetSingleRequest(request, project.Name);
                var subResult = await GetResultAsync(singleRequest, cancellationToken);
                result.VariableGroups.AddRange(subResult.VariableGroups);

                if (subResult.Status != Status.Success)
                {
                    result.Status = subResult.Status;
                }
            }
        }

        return Ok(result);
    }

    private static VariableGroupGetResponses GetVariableGroupGetResponses()
    {
        return new VariableGroupGetResponses
        {
            Status = Status.Success,
            VariableGroups = new List<VariableGroupGetResponse>()
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

    private async Task<VariableGroupGetResponses> GetResultAsync(VariableGroupDeleteRequest request, CancellationToken cancellationToken)
    {
        var vgServiceModel = _mapper.Map<VariableGroupDeleteModel>(request);

        _vgService.SetupConnectionRepository(vgServiceModel);
        await _vgService.DeleteVariableAsync(vgServiceModel, cancellationToken);
        var variableGroupResultModel = await _vgService.GetVariableGroupsAsync(vgServiceModel, cancellationToken);

        var result = _mapper.Map<VariableGroupGetResponses>(variableGroupResultModel);

        foreach (var variableGroupResponse in result.VariableGroups)
        {
            variableGroupResponse.Project = request.Project;
        }

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

        foreach (var variableGroupResponse in result.VariableGroups)
        {
            variableGroupResponse.Project = request.Project;
        }

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

        foreach (var variableGroupResponse in result.VariableGroups)
        {
            variableGroupResponse.Project = request.Project;
        }

        return result;
    }

    private async Task<VariableGroupGetResponses> GetResultAsync(VariableGroupGetRequest request, CancellationToken cancellationToken)
    {
        var vgServiceModel = _mapper.Map<VariableGroupGetModel>(request);

        _vgService.SetupConnectionRepository(vgServiceModel);
        var variableGroupResultModel = await _vgService.GetVariableGroupsAsync(vgServiceModel, cancellationToken);

        var result = _mapper.Map<VariableGroupGetResponses>(variableGroupResultModel);

        foreach (var variableGroupResponse in result.VariableGroups)
        {
            variableGroupResponse.Project = request.Project;
        }

        return result;
    }

    private static VariableGroupDeleteRequest GetSingleRequest(VariableGroupDeleteRequest request, string project)
    {
        return new VariableGroupDeleteRequest
        {
            Project = project,
            KeyFilter = request.KeyFilter,
            Organization = request.Organization,
            PAT = request.PAT,
            ValueFilter = request.ValueFilter,
            ContainsSecrets = request.ContainsSecrets,
            VariableGroupFilter = request.VariableGroupFilter
        };
    }

    private static VariableGroupAddRequest GetSingleRequest(VariableGroupAddRequest request, string project)
    {
        return new VariableGroupAddRequest
        {
            Project = project,
            KeyFilter = request.KeyFilter,
            Organization = request.Organization,
            PAT = request.PAT,
            ValueFilter = request.ValueFilter,
            ContainsSecrets = request.ContainsSecrets,
            VariableGroupFilter = request.VariableGroupFilter,
            Key = request.Key,
            Value = request.Value
        };
    }

    private static VariableGroupGetRequest GetSingleRequest(VariableGroupGetRequest request, string project)
    {
        return new VariableGroupGetRequest
        {
            Project = project,
            KeyFilter = request.KeyFilter,
            Organization = request.Organization,
            PAT = request.PAT,
            ValueFilter = request.ValueFilter,
            ContainsSecrets = request.ContainsSecrets,
            VariableGroupFilter = request.VariableGroupFilter
        };
    }

    private static VariableGroupUpdateRequest GetSingleRequest(VariableGroupUpdateRequest request, string project)
    {
        return new VariableGroupUpdateRequest
        {
            Project = project,
            KeyFilter = request.KeyFilter,
            Organization = request.Organization,
            PAT = request.PAT,
            ValueFilter = request.ValueFilter,
            ContainsSecrets = request.ContainsSecrets,
            VariableGroupFilter = request.VariableGroupFilter,
            NewValue = request.NewValue
        };
    }
}
