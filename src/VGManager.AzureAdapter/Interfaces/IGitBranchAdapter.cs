using VGManager.Models.StatusEnums;

namespace VGManager.AzureAdapter.Interfaces;

public interface IGitBranchAdapter
{
    Task<(AdapterStatus, IEnumerable<string>)> GetAllAsync(
        string organization,
        string pat,
        string repositoryId,
        CancellationToken cancellationToken = default
        );
}
