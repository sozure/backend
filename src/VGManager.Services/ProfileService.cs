using Microsoft.VisualStudio.Services.Profile;
using System.Text.Json;
using VGManager.Adapter.Azure.Services.Requests;
using VGManager.Adapter.Client.Interfaces;
using VGManager.Adapter.Models.Kafka;
using VGManager.Adapter.Models.Response;
using VGManager.Services.Interfaces;

namespace VGManager.Services;

public class ProfileService : IProfileService
{
    private readonly IVGManagerAdapterClientService _clientService;

    public ProfileService(
        IVGManagerAdapterClientService clientService
        )
    {
        _clientService = clientService;
    }

    public async Task<Profile?> GetProfileAsync(string organization, string pat, CancellationToken cancellationToken = default)
    {
        var request = new BaseRequest()
        {
            Organization = organization,
            PAT = pat
        };

        (bool isSuccess, string response) = await _clientService.SendAndReceiveMessageAsync(
            CommandTypes.GetProfileRequest,
            JsonSerializer.Serialize(request),
            cancellationToken);

        if (!isSuccess)
        {
            return null!;
        }

        var result = JsonSerializer.Deserialize<BaseResponse<Profile?>>(response)?.Data;
        return result;
    }
}
