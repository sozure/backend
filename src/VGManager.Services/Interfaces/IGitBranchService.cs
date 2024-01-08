using VGManager.Models.StatusEnums;

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
