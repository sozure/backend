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
}
