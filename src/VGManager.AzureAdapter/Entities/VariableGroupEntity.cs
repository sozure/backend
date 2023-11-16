using Microsoft.TeamFoundation.DistributedTask.WebApi;

namespace VGManager.AzureAdapter.Entities;
public class VariableGroupEntity
{
    public AdapterStatus Status { get; set; }
    public IEnumerable<VariableGroup> VariableGroups { get; set; } = null!;
}
