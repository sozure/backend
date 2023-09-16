using VGManager.Services.Models.Projects;

namespace VGManager.Services.Interfaces;
public interface IProjectService
{
    Task<ProjectResult> GetProjectsAsync(ProjectModel projectModel, CancellationToken cancellationToken = default);
}
