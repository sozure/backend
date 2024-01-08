using Microsoft.TeamFoundation.DistributedTask.WebApi;
using VGManager.Models;

namespace VGManager.Services.Models.VariableGroups.Results;

public class VariableGroupResults
{
    public AdapterStatus Status { get; set; }

    public IEnumerable<VariableGroup> VariableGroups { get; set; } = null!;
}
