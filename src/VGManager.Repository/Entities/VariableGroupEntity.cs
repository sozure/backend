using Microsoft.TeamFoundation.DistributedTask.WebApi;

namespace VGManager.Repository.Entities;
public class VariableGroupEntity
{
    public Status Status { get; set; }
    public IEnumerable<VariableGroup> VariableGroups { get; set; } = null!;
}
