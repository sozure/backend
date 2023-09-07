using VGManager.AzureAdapter.Entities;

namespace VGManager.Api.Projects.Responses;

public class ProjectsResponse
{
    public Status Status { get; set; }
    public IEnumerable<ProjectResponse> Projects { get; set; } = null!;
}
