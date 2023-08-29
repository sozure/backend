using Microsoft.TeamFoundation.DistributedTask.WebApi;

namespace VGManager.Repository.Interfaces;

public interface IVariableGroupConnectionRepository
{
    void Setup(string organization, string project, string pat);
    Task<IEnumerable<VariableGroup>> GetAll(CancellationToken cancellationToken = default);
    Task Update(VariableGroupParameters variableGroupParameters, int variableGroupId, CancellationToken cancellationToken = default);
}
