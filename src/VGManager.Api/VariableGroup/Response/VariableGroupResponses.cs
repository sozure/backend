using VGManager.AzureAdapter.Entities;

namespace VGManager.Api.VariableGroup.Response;

public class VariableGroupResponses
{
    public Status Status { get; set; }
    public List<VariableGroupResponse> VariableGroups { get; set; } = null!;
}
