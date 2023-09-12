using Microsoft.TeamFoundation.DistributedTask.WebApi;
using VGManager.AzureAdapter.Entities;

namespace VGManager.AzureAdapter.Interfaces;

public interface IVariableGroupAdapter
{
    void Setup(string organization, string project, string pat);
    Task<VariableGroupEntity> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Status> UpdateAsync(VariableGroupParameters variableGroupParameters, int variableGroupId, CancellationToken cancellationToken = default);
}
