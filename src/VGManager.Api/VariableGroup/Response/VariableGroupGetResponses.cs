using VGManager.AzureAdapter.Entities;

namespace VGManager.Api.VariableGroups.Response;

public class VariableGroupGetResponses
{
    public Status Status { get; set; }
    public List<VariableGroupGetResponse> VariableGroups { get; set; } = null!;
}
