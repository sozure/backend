using VGManager.AzureAdapter.Entities;

namespace VGManager.Services.Models.VariableGroups.Results;

public class VariableGroupResultsModel
{
    public Status Status { get; set; }
    public List<VariableGroupResultBaseModel> VariableGroups { get; set; } = null!;
}
