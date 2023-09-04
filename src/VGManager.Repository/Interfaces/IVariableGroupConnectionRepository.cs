using Microsoft.TeamFoundation.DistributedTask.WebApi;
using VGManager.Repository.Entities;

namespace VGManager.Repository.Interfaces;

public interface IVariableGroupConnectionRepository
{
    void Setup(string organization, string project, string pat);
    Task<VariableGroupEntity> GetAll(CancellationToken cancellationToken = default);
    Task<Status> Update(VariableGroupParameters variableGroupParameters, int variableGroupId, CancellationToken cancellationToken = default);
}
