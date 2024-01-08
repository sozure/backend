using VGManager.AzureAdapter.Interfaces;
using VGManager.Models;
using VGManager.Services.Interfaces;

namespace VGManager.Services;

public class GitFileService : IGitFileService
{
    private readonly IGitFileAdapter _gitFileAdapter;

    public GitFileService(IGitFileAdapter gitFileAdapter)
    {
        _gitFileAdapter = gitFileAdapter;
    }

    public async Task<(AdapterStatus, IEnumerable<string>)> GetFilePathAsync(
        string organization,
        string pat,
        string repositoryId,
        string fileName,
        string branch,
        CancellationToken cancellationToken = default
        )
    {
        return await _gitFileAdapter.GetFilePathAsync(organization, pat, repositoryId, fileName, branch, cancellationToken);
    }

    public async Task<(AdapterStatus, IEnumerable<string>)> GetConfigFilesAsync(
        string organization,
        string pat,
        string repositoryId,
        string? extension,
        string branch,
        CancellationToken cancellationToken = default
        )
    {
        return await _gitFileAdapter.GetConfigFilesAsync(organization, pat, repositoryId, extension, branch, cancellationToken);
    }
}
