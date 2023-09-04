using Microsoft.TeamFoundation.Core.WebApi;
using VGManager.Repository.Entities;

namespace VGManager.Repository.Interfaces;
public interface IProjectRepository
{
    Task<ProjectEntity> GetProjects(string baseUrl, string pat, CancellationToken cancellationToken = default);
}
