using VGManager.Services.Models.Projects;

namespace VGManager.Services.Interfaces;
public interface IProjectService
{
    Task<ProjectResultModel> GetProjectsAsync(ProjectModel projectModel, CancellationToken cancellationToken = default);
}
