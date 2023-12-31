using Microsoft.TeamFoundation.SourceControl.WebApi;
using VGManager.AzureAdapter.Entities;

namespace VGManager.Services.Interfaces;

public interface IGitBranchService
{
    Task<(AdapterStatus, IEnumerable<TfvcBranch>)> GetAllAsync(
        string organization,
        string pat,
        string project,
        CancellationToken cancellationToken = default
        );
}
