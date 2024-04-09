using Microsoft.TeamFoundation.SourceControl.WebApi;
using VGManager.Adapter.Models.Models;
using VGManager.Adapter.Models.Response;
using VGManager.Services.Models;

namespace VGManager.Services.Interfaces;
public interface IGitPullRequestService
{
    Task<AdapterResponseModel<List<GitPRResponse>>> GetPRsAsync(
        PRRequest model,
        CancellationToken cancellationToken
        );
}
