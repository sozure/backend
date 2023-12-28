using Microsoft.TeamFoundation.SourceControl.WebApi;

namespace VGManager.AzureAdapter.Interfaces;

public interface IGitRepositoryAdapter
{
    Task<IEnumerable<GitRepository>> GetAllAsync(
        string organization,
        string project,
        string pat,
        CancellationToken cancellationToken = default
        );
}
