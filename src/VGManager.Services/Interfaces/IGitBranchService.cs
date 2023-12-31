using Microsoft.TeamFoundation.SourceControl.WebApi;
using VGManager.AzureAdapter.Entities;

namespace VGManager.Services.Interfaces;

public interface IGitBranchService
{
    Task<(AdapterStatus, IEnumerable<string>)> GetAllAsync(
        string organization,
        string pat,
        string repositoryId,
        CancellationToken cancellationToken = default
        );
}
