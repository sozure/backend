using Microsoft.TeamFoundation.SourceControl.WebApi;
using VGManager.AzureAdapter.Entities;

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
