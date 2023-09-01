using Microsoft.TeamFoundation.Core.WebApi;
using VGManager.Services.Models.Projects;

namespace VGManager.Services.Interfaces;
public interface IProjectService
{
    Task<ProjectResultModel> GetProjects(ProjectModel projectModel, CancellationToken cancellationToken = default);
}
