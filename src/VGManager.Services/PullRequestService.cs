using System.Text.Json;
using VGManager.Adapter.Models.Kafka;
using VGManager.Adapter.Models.Models;
using VGManager.Adapter.Models.Requests;
using VGManager.Adapter.Models.Response;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Services.Interfaces;

namespace VGManager.Services;


public class PullRequestService(
    IAdapterCommunicator adapterCommunicator
    ) : IPullRequestService
{
    public async Task<AdapterResponseModel<List<GitPRResponse>>> GetPullRequestsAsync(GitPRRequest model, CancellationToken cancellationToken)
    {
        (var isSuccess, var response) = await adapterCommunicator.CommunicateWithAdapterAsync(
            model,
            CommandTypes.GetPullRequestsRequest,
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

    public async Task<AdapterResponseModel<bool>> CreatePullRequestAsync(
        CreatePRRequest model,
        CancellationToken cancellationToken
        )
    {
        (var isSuccess, var response) = await adapterCommunicator.CommunicateWithAdapterAsync(
            model,
            CommandTypes.CreatePullRequestRequest,
            cancellationToken
            );

        if (!isSuccess)
        {
            return new AdapterResponseModel<bool>
            {
                Data = false,
                Status = AdapterStatus.Unknown
            };
        }

        var rawResult = JsonSerializer.Deserialize<BaseResponse<AdapterResponseModel<bool>>>(response)?.Data;

        if (rawResult is null)
        {
            return new AdapterResponseModel<bool>
            {
                Data = false,
                Status = AdapterStatus.Unknown
            };
        }

        return rawResult;
    }

    public async Task<AdapterResponseModel<bool>> CreatePullRequestsAsync(
        CreatePRsRequest model,
        CancellationToken cancellationToken
        )
    {
        (var isSuccess, var response) = await adapterCommunicator.CommunicateWithAdapterAsync(
            model,
            CommandTypes.CreatePullRequestsRequest,
            cancellationToken
            );

        if (!isSuccess)
        {
            return new AdapterResponseModel<bool>
            {
                Data = false,
                Status = AdapterStatus.Unknown
            };
        }

        var rawResult = JsonSerializer.Deserialize<BaseResponse<AdapterResponseModel<bool>>>(response)?.Data;

        if (rawResult is null)
        {
            return new AdapterResponseModel<bool>
            {
                Data = false,
                Status = AdapterStatus.Unknown
            };
        }

        return rawResult;
    }
}
