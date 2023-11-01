using VGManager.AzureAdapter.Entities;

namespace VGManager.Api.VariableGroups.Response;

public class VariableResponses
{
    public Status Status { get; set; }
    public List<VariableResponse> Variables { get; set; } = null!;
}
