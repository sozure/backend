using VGManager.AzureAdapter.Entities;

namespace VGManager.AzureAdapter.Interfaces;
public interface IProjectAdapter
{
    Task<ProjectEntity> GetProjects(string baseUrl, string pat, CancellationToken cancellationToken = default);
}
