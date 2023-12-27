using VGManager.Api.VariableGroup.Response;
using VGManager.Api.VariableGroups.Request;
using VGManager.Api.VariableGroups.Response;
using VGManager.AzureAdapter.Entities;
using VGManager.Services.Models.Projects;
using VGManager.Services.Models.VariableGroups.Requests;

namespace VGManager.Api.Controllers;

public partial class VariableGroupController
{
    private static VariableResponses GetEmptyVariablesGetResponses()
    {
        return new VariableResponses
        {
            Status = AdapterStatus.Success,
            Variables = new List<VariableResponse>()
        };
    }

    private static VariableGroupResponses GetEmptyVariableGroupGetResponses()
    {
        return new VariableGroupResponses
        {
            Status = AdapterStatus.Success,
            VariableGroups = new List<VariableGroupResponse>()
        };
    }

    private async Task<ProjectsResult> GetProjectsAsync(VariableGroupRequest request, CancellationToken cancellationToken)
    {
        var projectModel = new ProjectModel
        {
            Organization = request.Organization,
            PAT = request.PAT
        };

        var projectResponse = await _projectService.GetProjectsAsync(projectModel, cancellationToken);
        return projectResponse;
    }

    private async Task<VariableResponses> GetResultAfterDeleteAsync(
        VariableGroupRequest request,
        CancellationToken cancellationToken
        )
    {
        var vgServiceModel = _mapper.Map<VariableGroupModel>(request);

        _vgService.SetupConnectionRepository(vgServiceModel);
        var status = await _vgService.DeleteVariablesAsync(vgServiceModel, true, cancellationToken);
        var variableGroupResultModel = await _vgService.GetVariablesAsync(vgServiceModel, cancellationToken);

        var result = _mapper.Map<VariableResponses>(variableGroupResultModel);

        if (status != AdapterStatus.Success)
        {
            result.Status = status;
        }

        return result;
    }

    private async Task<VariableResponses> GetBaseResultAsync(VariableGroupRequest request, CancellationToken cancellationToken)
    {
        var vgServiceModel = _mapper.Map<VariableGroupModel>(request);

        _vgService.SetupConnectionRepository(vgServiceModel);
        var variableGroupResultsModel = await _vgService.GetVariablesAsync(vgServiceModel, cancellationToken);

        var result = _mapper.Map<VariableResponses>(variableGroupResultsModel);
        return result;
    }

    private async Task<VariableGroupResponses> GetVGResultAsync(VariableGroupRequest request, CancellationToken cancellationToken)
    {
        var vgServiceModel = _mapper.Map<VariableGroupModel>(request);

        _vgService.SetupConnectionRepository(vgServiceModel);
        var variableGroupResultsModel = await _vgService.GetVariableGroupsAsync(vgServiceModel, cancellationToken);

        var result = new List<VariableGroupResponse>();

        foreach(var variableGroup in variableGroupResultsModel.VariableGroups)
        {
            result.Add(new()
            {
                Project = request.Project,
                VariableGroupName = variableGroup.Name
            });
        }

        return new VariableGroupResponses
        {
            Status = variableGroupResultsModel.Status,
            VariableGroups = result
        };
    }

    private async Task<VariableResponses> GetAddResultAsync(VariableGroupAddRequest request, CancellationToken cancellationToken)
    {
        var vgServiceModel = _mapper.Map<VariableGroupAddModel>(request);

        _vgService.SetupConnectionRepository(vgServiceModel);
        var status = await _vgService.AddVariablesAsync(vgServiceModel, cancellationToken);
        vgServiceModel.KeyFilter = vgServiceModel.Key;
        vgServiceModel.ValueFilter = vgServiceModel.Value;
        var variableGroupResultModel = await _vgService.GetVariablesAsync(vgServiceModel, cancellationToken);

        var result = _mapper.Map<VariableResponses>(variableGroupResultModel);

        if (status != AdapterStatus.Success)
        {
            result.Status = status;
        }

        return result;
    }

    private async Task<VariableResponses> GetUpdateResultAsync(
        VariableGroupUpdateRequest request,
        CancellationToken cancellationToken
        )
    {
        var vgServiceModel = _mapper.Map<VariableGroupUpdateModel>(request);

        _vgService.SetupConnectionRepository(vgServiceModel);
        var status = await _vgService.UpdateVariableGroupsAsync(vgServiceModel, true, cancellationToken);

        vgServiceModel.ValueFilter = vgServiceModel.NewValue;
        var variableGroupResultModel = await _vgService.GetVariablesAsync(vgServiceModel, cancellationToken);

        var result = _mapper.Map<VariableResponses>(variableGroupResultModel);

        if (status != AdapterStatus.Success)
        {
            result.Status = status;
        }

        return result;
    }

    private async Task<VariableResponses> GetVariableGroupResponsesAsync<T>(
        T request,
        CancellationToken cancellationToken
        )
    {
        VariableResponses? result;
        var vgRequest = request as VariableGroupRequest ?? new VariableGroupRequest();
        if (vgRequest.Project == "All")
        {
            result = GetEmptyVariablesGetResponses();
            var projectResponse = await GetProjectsAsync(vgRequest, cancellationToken);

            foreach (var project in projectResponse.Projects)
            {
                vgRequest.Project = project.Project.Name;
                var subResult = await GetResultAsync(request, vgRequest, cancellationToken);
                result.Variables.AddRange(subResult.Variables);

                if (subResult.Status != AdapterStatus.Success)
                {
                    result.Status = subResult.Status;
                }
            }
        }
        else
        {
            result = await GetResultAsync(request, vgRequest, cancellationToken);
        }
        return result;
    }

    private async Task<VariableResponses> GetResultAsync<T>(
        T request,
        VariableGroupRequest vgRequest,
        CancellationToken cancellationToken
        )
    {
        if (request is VariableGroupUpdateRequest updateRequest)
        {
            return await GetUpdateResultAsync(updateRequest, cancellationToken);
        }
        else if (request is VariableGroupAddRequest addRequest)
        {
            return await GetAddResultAsync(addRequest, cancellationToken);
        }
        else
        {
            return await GetBaseResultAsync(vgRequest, cancellationToken);
        }
    }
}
