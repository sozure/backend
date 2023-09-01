using VGManager.Services.Models.VariableGroups;
using VGManager.Services.Models.VariableGroups.Results;

namespace VGManager.Services.Interfaces;

public interface IVariableGroupService
{
    void SetupConnectionRepository(VariableGroupModel variableGroupModel);

    Task UpdateVariableGroupsAsync(VariableGroupUpdateModel variableGroupUpdateModel, CancellationToken cancellationToken = default);

    Task<IEnumerable<VariableGroupResultModel>> GetVariableGroupsAsync(
        VariableGroupModel variableGroupModel,
        CancellationToken cancellationToken = default
        );

    Task AddVariableAsync(VariableGroupAddModel variableGroupAddModel, CancellationToken cancellationToken = default);

    Task DeleteVariableAsync(VariableGroupDeleteModel variableGroupDeleteModel, CancellationToken cancellationToken = default);
}
