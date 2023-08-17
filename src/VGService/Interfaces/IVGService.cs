using VGService.Model;
using VGService.Repositories.Interface;

namespace VGService.Interfaces;

public interface IVariableGroupService
{
    public Task UpdateVariableGroupsAsync(
        IVariableGroupConnectionRepository connectionService,
        string variableGroupFilter, string keyFilter,
        string newValue, string valueCondition
        );

    public Task<IEnumerable<MatchedVariableGroup>> GetVariableGroupsAsync(IVariableGroupConnectionRepository connectionService,
        string variableGroupFilter, string keyFilter, string valueFilter);

    public Task AddVariableAsync(IVariableGroupConnectionRepository connectionService,
        string variableGroupFilter, string keyFilter, string key,
        string newValue);

    public Task DeleteVariableAsync(IVariableGroupConnectionRepository connectionService,
        string variableGroupFilter, string keyFilter, string valueCondition);
}
