using System.Text.Json;
using VGManager.Adapter.Models.Response;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Services.Interfaces;

namespace VGManager.Services.Helper;

public class GitAdapterCommunicatorService(
    IAdapterCommunicator adapterCommunicator
        ) : IGitAdapterCommunicatorService
{
    public async Task<(AdapterStatus, IEnumerable<string>)> GetInformationAsync<T>(
        string commandType,
        T request,
        CancellationToken cancellationToken
        )
    {
        (var isSuccess, var response) = await adapterCommunicator.CommunicateWithAdapterAsync(
            request,
            commandType,
            cancellationToken
            );

        if (!isSuccess)
        {
            return (AdapterStatus.Unknown, Enumerable.Empty<string>());
        }

        var result = JsonSerializer.Deserialize<BaseResponse<Dictionary<string, object>>>(response)?.Data;

        if (result is null)
        {
            return (AdapterStatus.Unknown, Enumerable.Empty<string>());
        }

        var isParseCompleted = int.TryParse(result["Status"].ToString(), out var i);

        if (!isParseCompleted)
        {
            return (AdapterStatus.Unknown, Enumerable.Empty<string>());
        }

        var status = (AdapterStatus)i;
        var res = JsonSerializer.Deserialize<List<string>>(result["Data"].ToString() ?? "[]") ?? [];
        return (status, res);
    }
}
