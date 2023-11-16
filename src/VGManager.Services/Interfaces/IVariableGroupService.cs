using VGManager.AzureAdapter.Entities;
using VGManager.Services.Models.VariableGroups.Requests;
using VGManager.Services.Models.VariableGroups.Results;

namespace VGManager.Services.Interfaces;

public interface IVariableGroupService
{
    void SetupConnectionRepository(VariableGroupModel variableGroupModel);

    Task<AdapterStatus> UpdateVariableGroupsAsync(
        string userName,
        VariableGroupUpdateModel variableGroupUpdateModel,
        bool filterAsRegex,
        CancellationToken cancellationToken = default
        );

    Task<VariableResults> GetVariableGroupsAsync(
        VariableGroupModel variableGroupModel,
        CancellationToken cancellationToken = default
        );

    Task<AdapterStatus> AddVariablesAsync(
        string userName, 
        VariableGroupAddModel variableGroupAddModel, 
        CancellationToken cancellationToken = default
        );

    Task<AdapterStatus> DeleteVariablesAsync(
        string userName,
        VariableGroupModel variableGroupModel,
        bool filterAsRegex,
        CancellationToken cancellationToken = default
        );
}
