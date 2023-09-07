using Microsoft.TeamFoundation.DistributedTask.WebApi;
using VGManager.AzureAdapter.Entities;

namespace VGManager.AzureAdapter.Interfaces;

public interface IVariableGroupAdapter
{
    void Setup(string organization, string project, string pat);
    Task<VariableGroupEntity> GetAll(CancellationToken cancellationToken = default);
    Task<Status> Update(VariableGroupParameters variableGroupParameters, int variableGroupId, CancellationToken cancellationToken = default);
}
