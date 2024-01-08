
using VGManager.Models;

namespace VGManager.AzureAdapter.Interfaces;

public interface IGitBranchAdapter
{
    void Setup(string organization, string pat);
    Task<(AdapterStatus, IEnumerable<string>)> GetAllAsync(
        string organization,
        string pat,
        string repositoryId,
        CancellationToken cancellationToken = default
        );
}
