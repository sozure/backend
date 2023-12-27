using VGManager.AzureAdapter.Entities;

namespace VGManager.AzureAdapter.Interfaces;
public interface IProjectAdapter
{
    Task<ProjectsEntity> GetProjectsAsync(string baseUrl, string pat, CancellationToken cancellationToken = default);
}
