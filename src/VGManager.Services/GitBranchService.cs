using VGManager.AzureAdapter.Interfaces;
using VGManager.Models;
using VGManager.Services.Interfaces;

namespace VGManager.Services;

public class GitBranchService : IGitBranchService
{
    private readonly IGitBranchAdapter _gitBranchAdapter;

    public GitBranchService(IGitBranchAdapter gitBranchAdapter)
    {
        _gitBranchAdapter = gitBranchAdapter;
    }

    public async Task<(AdapterStatus, IEnumerable<string>)> GetAllAsync(
        string organization,
        string pat,
        string repositoryId,
        CancellationToken cancellationToken = default
        )
    {
        return await _gitBranchAdapter.GetAllAsync(organization, pat, repositoryId, cancellationToken);
    }
}
