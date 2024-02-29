using Microsoft.TeamFoundation.WorkItemTracking.Process.WebApi.Models;
using System.Data;
using System.Text.Json;
using VGManager.Adapter.Models.Kafka;
using VGManager.Adapter.Models.Requests;
using VGManager.Adapter.Models.Response;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Services.Interfaces;

namespace VGManager.Services;

public class GitFileService(
    IAdapterCommunicator adapterCommunicator
        ) : IGitFileService
{
    public async Task<(AdapterStatus, IEnumerable<string>)> GetFilePathAsync(
        string organization,
        string pat,
        string repositoryId,
        string fileName,
        string branch,
        CancellationToken cancellationToken = default
        )
    {
        var commandType = CommandTypes.GetFilePathRequest;

        return await GetInformationAsync(
            commandType,
            organization,
            pat,
            repositoryId,
            branch,
            fileName,
            cancellationToken
            );
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
        var commandType = CommandTypes.GetConfigFilesRequest;

        return await GetInformationAsync(
            commandType, 
            organization, 
            pat, 
            repositoryId, 
            branch, 
            extension, 
            cancellationToken
            );
    }

    private async Task<(AdapterStatus, IEnumerable<string>)> GetInformationAsync(
        string commandType,
        string organization, 
        string pat, 
        string repositoryId, 
        string branch,
        string? additionalInformation,
        CancellationToken cancellationToken
        )
    {
        var request = new GitFileBaseRequest<string>()
        {
            Organization = organization,
            PAT = pat,
            Branch = branch,
            RepositoryId = repositoryId,
            AdditionalInformation = additionalInformation,
        };

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
