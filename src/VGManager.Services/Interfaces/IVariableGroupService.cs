using VGManager.Services.Model;

namespace VGManager.Services.Interfaces;

public interface IVariableGroupService
{
    void SetupConnectionRepository(string organization, string project, string pat);
    public Task UpdateVariableGroupsAsync(string variableGroupFilter, string keyFilter, string newValue, string valueCondition);
    public Task<IEnumerable<MatchedVariableGroup>> GetVariableGroupsAsync(string variableGroupFilter, string keyFilter, string valueFilter);
    public Task AddVariableAsync(string variableGroupFilter, string keyFilter, string key, string newValue);
    public Task DeleteVariableAsync(string variableGroupFilter, string keyFilter, string valueCondition);
}
