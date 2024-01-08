using VGManager.AzureAdapter.Entities;
using VGManager.Models;

namespace VGManager.AzureAdapter.Interfaces;
public interface IProjectAdapter
{
    Task<AdapterResponseModel<IEnumerable<ProjectEntity>>> GetProjectsAsync(string baseUrl, string pat, CancellationToken cancellationToken = default);
}
