using Microsoft.TeamFoundation.SourceControl.WebApi;
using VGManager.AzureAdapter.Entities;
using VGManager.AzureAdapter.Interfaces;
using VGManager.Services.Interfaces;

namespace VGManager.Services;

public class GitBranchService: IGitBranchService
{
    private readonly IGitBranchAdapter _gitBranchAdapter;

    public GitBranchService(IGitBranchAdapter gitBranchAdapter)
    {
        _gitBranchAdapter = gitBranchAdapter;
    }

    public async Task<(AdapterStatus, IEnumerable<TfvcBranch>)> GetAllAsync(
        string organization,
        string pat,
        string project,
        CancellationToken cancellationToken = default
        )
    {
        return await _gitBranchAdapter.GetAllAsync(organization, pat, project, cancellationToken);
    }
}
