using VGManager.Adapter.Models.Models;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Services.Models;

namespace VGManager.Services.Interfaces;

public interface IGitVersionService
{
    Task<(AdapterStatus, IEnumerable<string>)> GetBranchesAsync(
        string organization,
        string pat,
        string repositoryId,
        CancellationToken cancellationToken = default
        );

    Task<(AdapterStatus, IEnumerable<string>)> GetTagsAsync(
        string organization,
        string pat,
        Guid repositoryId,
        CancellationToken cancellationToken = default
        );

    Task<AdapterResponseModel<Dictionary<string, string>>> GetLatestTagsAsync(
        GitLatestTagsEntity model,
        CancellationToken cancellationToken = default
        );

    Task<(AdapterStatus, string)> CreateTagAsync(
        CreateTagEntity tagEntity,
        CancellationToken cancellationToken = default
        );
}
