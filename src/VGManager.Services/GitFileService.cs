using System.Text.Json;
using VGManager.Adapter.Azure.Services.Requests;
using VGManager.Adapter.Client.Interfaces;
using VGManager.Adapter.Models.Kafka;
using VGManager.Adapter.Models.Response;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Services.Interfaces;

namespace VGManager.Services;

public class GitFileService : IGitFileService
{
    private readonly IVGManagerAdapterClientService _clientService;


    public GitFileService(
        IVGManagerAdapterClientService clientService
        )
    {
        _clientService = clientService;
    }

    public async Task<(AdapterStatus, IEnumerable<string>)> GetFilePathAsync(
        string organization,
        string pat,
        string repositoryId,
        string fileName,
        string branch,
        CancellationToken cancellationToken = default
        )
    {
        var request = new GitFileBaseRequest<string>()
        {
            Organization = organization,
            PAT = pat,
            Branch = branch,
            RepositoryId = repositoryId,
            AdditionalInformation = fileName,
        };

        (bool isSuccess, string response) = await _clientService.SendAndReceiveMessageAsync(
            CommandTypes.GetFilePathRequest,
            JsonSerializer.Serialize(request),
            cancellationToken);

        if (!isSuccess)
        {
            return (AdapterStatus.Unknown, Enumerable.Empty<string>());
        }

        var result = JsonSerializer.Deserialize<BaseResponse<Dictionary<string, object>>>(response)?.Data;

        if (result is null)
        {
            return (AdapterStatus.Unknown, Enumerable.Empty<string>());
        }

        int.TryParse(result["Status"].ToString(), out var i);
        var status = (AdapterStatus)i;
        var res = JsonSerializer.Deserialize<List<string>>(result["Data"].ToString() ?? "[]") ?? [];
        return (status, res);
    }

    public async Task<(AdapterStatus, IEnumerable<string>)> GetConfigFilesAsync(
        string organization,
        string pat,
        string repositoryId,
        string? extension,
        string branch,
        CancellationToken cancellationToken = default
        )
    {
        var request = new GitFileBaseRequest<string>()
        {
            Organization = organization,
            PAT = pat,
            Branch = branch,
            RepositoryId = repositoryId,
            AdditionalInformation = extension,
        };

        (bool isSuccess, string response) = await _clientService.SendAndReceiveMessageAsync(
            CommandTypes.GetConfigFilesRequest,
            JsonSerializer.Serialize(request),
            cancellationToken);

        if (!isSuccess)
        {
            return (AdapterStatus.Unknown, Enumerable.Empty<string>());
        }

        var result = JsonSerializer.Deserialize<BaseResponse<Dictionary<string, object>>>(response)?.Data;

        if (result is null)
        {
            return (AdapterStatus.Unknown, Enumerable.Empty<string>());
        }

        int.TryParse(result["Status"].ToString(), out var i);
        var status = (AdapterStatus)i;
        var res = JsonSerializer.Deserialize<List<string>>(result["Data"].ToString() ?? "[]") ?? [];
        return (status, res);
    }
}
