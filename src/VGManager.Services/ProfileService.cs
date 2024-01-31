using Microsoft.VisualStudio.Services.Profile;
using System.Text.Json;
using VGManager.Adapter.Models.Kafka;
using VGManager.Adapter.Models.Requests;
using VGManager.Adapter.Models.Response;
using VGManager.Services.Interfaces;

namespace VGManager.Services;

public class ProfileService : IProfileService
{
    private readonly IAdapterCommunicator _adapterCommunicator;

    public ProfileService(
        IAdapterCommunicator adapterCommunicator
        )
    {
        _adapterCommunicator = adapterCommunicator;
    }

    public async Task<Profile?> GetProfileAsync(string organization, string pat, CancellationToken cancellationToken = default)
    {
        var request = new BaseRequest()
        {
            Organization = organization,
            PAT = pat
        };

        (var isSuccess, var response) = await _adapterCommunicator.CommunicateWithAdapterAsync(
            request,
            CommandTypes.GetProfileRequest,
            cancellationToken
            );

        if (!isSuccess)
        {
            return null!;
        }

        var result = JsonSerializer.Deserialize<BaseResponse<Profile?>>(response)?.Data;
        return result;
    }
}
