using VGManager.Adapter.Models.Models;
using VGManager.Services.Models;

namespace VGManager.Services.Interfaces;
public interface IGitPullRequestService
{
    Task<AdapterResponseModel<List<GitPRResponse>>> GetPRsAsync(
        GitPRRequest model,
        CancellationToken cancellationToken
        );

    Task<AdapterResponseModel<bool>> CreatePullRequestAsync(
        CreatePRRequest model,
        CancellationToken cancellationToken
        );

    Task<AdapterResponseModel<bool>> CreatePullRequestsAsync(
        CreatePRsRequest model,
        CancellationToken cancellationToken
        );
}
