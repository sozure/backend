using VGManager.AzureAdapter.Entities;

namespace VGManager.Services.Models.VariableGroups.Results;

public class VariableResults
{
    public AdapterStatus Status { get; set; }
    public IEnumerable<VariableResult> Variables { get; set; } = null!;
}
