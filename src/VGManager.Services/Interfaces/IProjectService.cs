using VGManager.Services.Models.Projects;

namespace VGManager.Services.Interfaces;
public interface IProjectService
{
    Task<ProjectsResult> GetProjectsAsync(ProjectModel projectModel, CancellationToken cancellationToken = default);
}
