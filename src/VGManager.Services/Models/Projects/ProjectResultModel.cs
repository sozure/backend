using Microsoft.TeamFoundation.Core.WebApi;
using VGManager.Repository.Entities;

namespace VGManager.Services.Models.Projects;
public class ProjectResultModel
{
    public Status Status { get; set; }
    public IEnumerable<TeamProjectReference> Projects { get; set; } = null!;
}
