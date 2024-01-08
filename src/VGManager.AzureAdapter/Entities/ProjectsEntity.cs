using VGManager.Models;

namespace VGManager.AzureAdapter.Entities;

public class ProjectsEntity
{
    public AdapterStatus Status { get; set; }
    public IEnumerable<ProjectEntity> Projects { get; set; } = null!;
}
