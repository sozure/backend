using Microsoft.TeamFoundation.SourceControl.WebApi;
using VGManager.AzureAdapter.Entities;

namespace VGManager.AzureAdapter.Interfaces;

public interface IGitRepositoryAdapter
{
    void Setup(string organization, string pat);

    Task<IEnumerable<GitRepository>> GetAllAsync(
        string organization,
        string project,
        string pat,
        CancellationToken cancellationToken = default
        );

    Task<List<string>> GetVariablesFromConfigAsync(
        GitRepositoryEntity gitRepositoryEntity,
        CancellationToken cancellationToken = default
        );
}
