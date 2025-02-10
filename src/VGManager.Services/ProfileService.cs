using System.Text.Json;
using Microsoft.VisualStudio.Services.Profile;
using VGManager.Adapter.Models.Kafka;
using VGManager.Adapter.Models.Requests;
using VGManager.Adapter.Models.Response;
using VGManager.Services.Interfaces;

namespace VGManager.Services;

public class ProfileService(
    IAdapterCommunicator adapterCommunicator
        ) : IProfileService
{
    public async Task<Profile?> GetProfileAsync(string organization, string pat, CancellationToken cancellationToken = default)
    {
        var request = new BaseRequest()
        {
            Organization = organization,
            PAT = pat
        };

        (var isSuccess, var response) = await adapterCommunicator.CommunicateWithAdapterAsync(
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
