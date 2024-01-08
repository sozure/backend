using VGManager.Models.Models;
using VGManager.Services.Models.Projects;

namespace VGManager.Services.Interfaces;
public interface IProjectService
{
    Task<AdapterResponseModel<IEnumerable<ProjectResult>>> GetProjectsAsync(ProjectModel projectModel, CancellationToken cancellationToken = default);
}
