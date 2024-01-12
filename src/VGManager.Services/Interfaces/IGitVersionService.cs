using VGManager.Models.StatusEnums;

namespace VGManager.Services.Interfaces;

public interface IGitVersionService
{
    Task<(AdapterStatus, IEnumerable<string>)> GetBranchesAsync(
        string organization,
        string pat,
        string repositoryId,
        CancellationToken cancellationToken = default
        );
}
