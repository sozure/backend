using VGManager.Services.Model;
using VGManager.Services.Repositories.Interface;

namespace VGManager.Services.Interfaces;

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
