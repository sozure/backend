using VGManager.AzureAdapter.Entities;

namespace VGManager.Api.VariableGroups.Response;

public class VariableGroupResponses
{
    public Status Status { get; set; }
    public List<VariableGroupResponse> VariableGroups { get; set; } = null!;
}
