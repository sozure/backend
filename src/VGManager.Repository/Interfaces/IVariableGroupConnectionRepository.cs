using Microsoft.TeamFoundation.DistributedTask.WebApi;

namespace VGManager.Repository.Interfaces;

public interface IVariableGroupConnectionRepository
{
    public void Setup(string organization, string project, string pat);
    public Task<IEnumerable<VariableGroup>> GetAll();
    public Task Update(VariableGroupParameters variableGroupParameters, int variableGroupId);
}
