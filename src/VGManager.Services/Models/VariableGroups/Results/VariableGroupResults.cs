using VGManager.AzureAdapter.Entities;

namespace VGManager.Services.Models.VariableGroups.Results;

public class VariableGroupResults
{
    public Status Status { get; set; }
    public List<VariableGroupResult> VariableGroups { get; set; } = null!;
}
