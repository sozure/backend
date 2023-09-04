using VGManager.Repository.Entities;

namespace VGManager.Services.Models.VariableGroups.Results;

public class VariableGroupResultsModel
{
    public Status Status { get; set; }
    public IEnumerable<VariableGroupResultModel> VariableGroups { get; set; } = null!;
}
