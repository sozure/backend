using Microsoft.TeamFoundation.Core.WebApi;
using VGManager.Services.Models;

namespace VGManager.Services.Interfaces;
public interface IProjectService
{
    Task<IEnumerable<TeamProjectReference>> GetProjects(ProjectModel projectModel, CancellationToken cancellationToken = default);
}
