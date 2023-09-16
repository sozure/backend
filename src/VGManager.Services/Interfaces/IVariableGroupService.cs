using VGManager.AzureAdapter.Entities;
using VGManager.Services.Models.VariableGroups.Requests;
using VGManager.Services.Models.VariableGroups.Results;

namespace VGManager.Services.Interfaces;

public interface IVariableGroupService
{
    void SetupConnectionRepository(VariableGroupModel variableGroupModel);

    Task<Status> UpdateVariableGroupsAsync(VariableGroupUpdateModel variableGroupUpdateModel, CancellationToken cancellationToken = default);

    Task<VariableGroupResultsModel> GetVariableGroupsAsync(
        VariableGroupModel variableGroupModel,
        CancellationToken cancellationToken = default
        );

    Task<Status> AddVariablesAsync(VariableGroupAddModel variableGroupAddModel, CancellationToken cancellationToken = default);

    Task<Status> DeleteVariableAsync(VariableGroupModel variableGroupModel, CancellationToken cancellationToken = default);
}
