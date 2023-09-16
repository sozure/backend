using Microsoft.TeamFoundation.Core.WebApi;
using VGManager.AzureAdapter.Entities;

namespace VGManager.Services.Models.Projects;
public class ProjectResult
{
    public Status Status { get; set; }
    public IEnumerable<TeamProjectReference> Projects { get; set; } = null!;
}
