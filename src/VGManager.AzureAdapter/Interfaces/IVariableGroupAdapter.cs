using Microsoft.TeamFoundation.DistributedTask.WebApi;
using VGManager.AzureAdapter.Entities;
using VGManager.Models;

namespace VGManager.AzureAdapter.Interfaces;

public interface IVariableGroupAdapter
{
    void Setup(string organization, string project, string pat);
    Task<AdapterResponseModel<IEnumerable<VariableGroup>>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<AdapterStatus> UpdateAsync(VariableGroupParameters variableGroupParameters, int variableGroupId, CancellationToken cancellationToken = default);
}
