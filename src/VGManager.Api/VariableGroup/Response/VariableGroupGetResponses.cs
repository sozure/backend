using VGManager.Api.VariableGroup.Response;
using VGManager.AzureAdapter.Entities;

namespace VGManager.Api.VariableGroups.Response;

public class VariableGroupGetResponses
{
    public Status Status { get; set; }
    public List<VariableGroupGetBaseResponse> VariableGroups { get; set; } = null!;
}
