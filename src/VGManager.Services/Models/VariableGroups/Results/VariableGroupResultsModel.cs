using VGManager.AzureAdapter.Entities;

namespace VGManager.Services.Models.VariableGroups.Results;

public class VariableGroupResultsModel
{
    public Status Status { get; set; }
    public List<VariableGroupBaseResultModel> VariableGroups { get; set; } = null!;
}
