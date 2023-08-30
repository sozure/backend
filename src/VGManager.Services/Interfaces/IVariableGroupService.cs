using VGManager.Services.Models;
using VGManager.Services.Models.MatchedModels;

namespace VGManager.Services.Interfaces;

public interface IVariableGroupService
{
    void SetupConnectionRepository(VariableGroupModel variableGroupModel);
    Task UpdateVariableGroupsAsync(
        string variableGroupFilter,
        string keyFilter,
        string newValue,
        string valueCondition,
        CancellationToken cancellationToken = default
        );
    Task<IEnumerable<MatchedVariableGroup>> GetVariableGroupsAsync(
        VariableGroupGetModel variableGroupGetModel,
        CancellationToken cancellationToken = default
        );
    Task AddVariableAsync(
        string variableGroupFilter,
        string keyFilter,
        string key,
        string newValue,
        CancellationToken cancellationToken = default
        );
    Task DeleteVariableAsync(
        string variableGroupFilter,
        string keyFilter,
        string valueCondition,
        CancellationToken cancellationToken = default
        );
}
