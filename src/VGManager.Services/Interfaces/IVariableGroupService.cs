using VGManager.Services.Models;

namespace VGManager.Services.Interfaces;

public interface IVariableGroupService
{
    void SetupConnectionRepository(string organization, string project, string pat);
    Task UpdateVariableGroupsAsync(
        string variableGroupFilter,
        string keyFilter,
        string newValue,
        string valueCondition,
        CancellationToken cancellationToken = default
        );
    Task<IEnumerable<MatchedVariableGroup>> GetVariableGroupsAsync(
        string variableGroupFilter,
        string keyFilter,
        string valueFilter,
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
