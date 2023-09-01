using VGManager.Repository.Entities;
using VGManager.Services.Models.VariableGroups;
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

    Task<Status> AddVariableAsync(VariableGroupAddModel variableGroupAddModel, CancellationToken cancellationToken = default);

    Task<Status> DeleteVariableAsync(VariableGroupDeleteModel variableGroupDeleteModel, CancellationToken cancellationToken = default);
}
