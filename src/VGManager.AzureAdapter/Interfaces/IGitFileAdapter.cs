using VGManager.AzureAdapter.Entities;

namespace VGManager.AzureAdapter.Interfaces;

public interface IGitFileAdapter
{
    Task<(AdapterStatus, IEnumerable<string>)> GetFilePathAsync(
        string organization,
        string pat,
        string repositoryId,
        string fileName,
        string branch,
        CancellationToken cancellationToken = default
        );
}
