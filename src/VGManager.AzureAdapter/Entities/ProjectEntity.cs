using Microsoft.TeamFoundation.Core.WebApi;

namespace VGManager.AzureAdapter.Entities;
public class ProjectEntity
{
    public AdapterStatus Status { get; set; }
    public IEnumerable<TeamProjectReference> Projects { get; set; } = null!;
}
