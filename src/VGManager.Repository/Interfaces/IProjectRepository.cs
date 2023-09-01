using Microsoft.TeamFoundation.Core.WebApi;

namespace VGManager.Repository.Interfaces;
public interface IProjectRepository
{
    Task<IEnumerable<TeamProjectReference>> GetProjects(string baseUrl, string pat, CancellationToken cancellationToken = default);
}
