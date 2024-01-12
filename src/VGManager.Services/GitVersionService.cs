using VGManager.AzureAdapter.Interfaces;
using VGManager.Models.StatusEnums;
using VGManager.Services.Interfaces;

namespace VGManager.Services;

public class GitVersionService : IGitVersionService
{
    private readonly IGitVersionAdapter _gitBranchAdapter;

    public GitVersionService(IGitVersionAdapter gitBranchAdapter)
    {
        _gitBranchAdapter = gitBranchAdapter;
    }

    public async Task<(AdapterStatus, IEnumerable<string>)> GetBranchesAsync(
        string organization,
        string pat,
        string repositoryId,
        CancellationToken cancellationToken = default
        )
    {
        return await _gitBranchAdapter.GetBranchesAsync(organization, pat, repositoryId, cancellationToken);
    }
}
