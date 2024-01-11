using Microsoft.TeamFoundation.DistributedTask.WebApi;
using VGManager.Models.Models;
using VGManager.Services.Models.VariableGroups.Requests;

namespace VGManager.Services.Interfaces;

public interface IVariableGroupService
{
    Task<AdapterResponseModel<IEnumerable<VariableGroup>>> GetVariableGroupsAsync(
        VariableGroupModel variableGroupModel,
        bool containsKey,
        CancellationToken cancellationToken = default
        );
}
