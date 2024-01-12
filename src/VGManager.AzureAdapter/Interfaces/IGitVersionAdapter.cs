using VGManager.Models.StatusEnums;

namespace VGManager.AzureAdapter.Interfaces;

public interface IGitVersionAdapter
{
    Task<(AdapterStatus, IEnumerable<string>)> GetBranchesAsync(
        string organization,
        string pat,
        string repositoryId,
        CancellationToken cancellationToken = default
        );
    Task<(AdapterStatus, IEnumerable<string>)> GetTagsAsync(
        string organization,
        string pat,
        Guid repositoryId,
        CancellationToken cancellationToken = default
        );
}
