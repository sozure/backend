using Microsoft.TeamFoundation.SourceControl.WebApi;
using System.Text.Json;
using VGManager.Adapter.Models.Models;
using VGManager.Adapter.Models.Response;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Services.Interfaces;
using VGManager.Services.Models;

namespace VGManager.Services;


public class GitPullRequestService(
    IAdapterCommunicator adapterCommunicator
    ) : IGitPullRequestService
{
    public async Task<AdapterResponseModel<List<GitPRResponse>>> GetPRsAsync(PRRequest model, CancellationToken cancellationToken)
    {
        (var isSuccess, var response) = await adapterCommunicator.CommunicateWithAdapterAsync(
            model,
            "GetPullRequestsRequest",
            cancellationToken
            );

        if (!isSuccess)
        {
            return new AdapterResponseModel<List<GitPRResponse>>
            {
                Data = [],
                Status = AdapterStatus.Unknown
            };
        }

        var rawResult = JsonSerializer.Deserialize<BaseResponse<AdapterResponseModel<List<GitPRResponse>>>>(response)?.Data;

        if (rawResult is null)
        {
            return new AdapterResponseModel<List<GitPRResponse>>
            {
                Data = [],
                Status = AdapterStatus.Unknown
            };
        }

        return rawResult;
    }
}
