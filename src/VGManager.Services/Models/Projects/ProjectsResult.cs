using VGManager.Models;

namespace VGManager.Services.Models.Projects;
public class ProjectsResult
{
    public AdapterStatus Status { get; set; }
    public IEnumerable<ProjectResult> Projects { get; set; } = null!;
}
