using Microsoft.TeamFoundation.Core.WebApi;
using VGManager.AzureAdapter.Entities;

namespace VGManager.Services.Models.Projects;
public class ProjectResult
{
    public AdapterStatus Status { get; set; }
    public IEnumerable<TeamProjectReference> Projects { get; set; } = null!;
}
