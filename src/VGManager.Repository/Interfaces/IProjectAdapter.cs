using VGManager.Repository.Entities;

namespace VGManager.Repository.Interfaces;
public interface IProjectAdapter
{
    Task<ProjectEntity> GetProjects(string baseUrl, string pat, CancellationToken cancellationToken = default);
}
