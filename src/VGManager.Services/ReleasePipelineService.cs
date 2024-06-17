using Microsoft.Extensions.Logging;
using System.Text.Json;
using VGManager.Adapter.Models.Kafka;
using VGManager.Adapter.Models.Requests;
using VGManager.Adapter.Models.Response;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Services.Interfaces;

namespace VGManager.Services;

public class ReleasePipelineService(
    IAdapterCommunicator adapterCommunicator,
    ILogger<ReleasePipelineService> logger
        ) : IReleasePipelineService
{
    public async Task<(AdapterStatus, IEnumerable<string>)> GetProjectsWithReleasePipelineAsync(
        string organization,
        string pat,
        IEnumerable<string> projects,
        string repositoryName,
        string configFile,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            var adapterResult = await GetEnvironmentsFromMultipleProjectsAsync(
                organization, 
                pat, 
                projects, 
                repositoryName, 
                configFile, 
                cancellationToken
                );
            var status = adapterResult.Item1;
            var result = adapterResult.Item2;
            return (status, result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting projects with release pipelines for {repository} repository.", repositoryName);
            return (AdapterStatus.Unknown, []);
        }
    }

    public async Task<(AdapterStatus, IEnumerable<string>)> GetEnvironmentsAsync(
        string organization,
        string pat,
        string project,
        string repositoryName,
        string configFile,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            logger.LogInformation("Request release environments for {repository} repository.", repositoryName);
            var request = new ReleasePipelineRequest()
            {
                Organization = organization,
                PAT = pat,
                ConfigFile = configFile,
                Project = project,
                RepositoryName = repositoryName
            };

            (var isSuccess, var response) = await adapterCommunicator.CommunicateWithAdapterAsync(
                request,
                CommandTypes.GetEnvironmentsRequest,
                cancellationToken
                );

            if (!isSuccess)
            {
                return (AdapterStatus.Unknown, []);
            }

            var adapterResult = JsonSerializer.Deserialize<BaseResponse<Dictionary<string, object>>>(response)?.Data;

            if (adapterResult is null)
            {
                return (AdapterStatus.Unknown, []);
            }

            var isParseCompleted = int.TryParse(adapterResult["Status"].ToString(), out var i);

            if (!isParseCompleted)
            {
                return (AdapterStatus.Unknown, []);
            }

            var status = (AdapterStatus)i;
            var res = JsonSerializer.Deserialize<List<string>>(adapterResult["Data"].ToString() ?? "[]") ?? [];
            return (status, res);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting release environments for {repository} repository.", repositoryName);
            return (AdapterStatus.Unknown, []);
        }
    }

    public async Task<(AdapterStatus, IEnumerable<(string, string)>)> GetVariableGroupsAsync(
        string organization,
        string pat,
        string project,
        string repositoryName,
        string configFile,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            logger.LogInformation("Request variable groups connected to release pipeline for {repository} repository.", repositoryName);
            var request = new ReleasePipelineRequest()
            {
                Organization = organization,
                PAT = pat,
                ConfigFile = configFile,
                Project = project,
                RepositoryName = repositoryName
            };

            (var isSuccess, var response) = await adapterCommunicator.CommunicateWithAdapterAsync(
                request,
                CommandTypes.GetVariableGroupsRequest,
                cancellationToken
                );

            if (!isSuccess)
            {
                return (AdapterStatus.Unknown, []);
            }

            var adapterResult = JsonSerializer.Deserialize<BaseResponse<Dictionary<string, object>>>(response)?.Data;

            if (adapterResult is null)
            {
                return (AdapterStatus.Unknown, []);
            }

            var isParseCompleted = int.TryParse(adapterResult["Status"].ToString(), out var i);

            if (!isParseCompleted)
            {
                return (AdapterStatus.Unknown, []);
            }

            var status = (AdapterStatus)i;
            var res = JsonSerializer.Deserialize<List<Dictionary<string, string>>>(adapterResult["Data"].ToString() ?? "[]") ?? [];
            var res2 = new List<(string, string)>();
            foreach (var element in res)
            {
                res2.Add((element["Name"], element["Type"]));
            }
            return (status, res2);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting variable groups connected to release pipeline for {repository} repository.", repositoryName);
            return (AdapterStatus.Unknown, []);
        }
    }

    private async Task<(AdapterStatus, IEnumerable<string>)> GetEnvironmentsFromMultipleProjectsAsync(
        string organization,
        string pat,
        IEnumerable<string> projects,
        string repositoryName,
        string configFile,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            logger.LogInformation("Request release environments for {repository} repository.", repositoryName);
            var request = new MultipleReleasePipelineRequest()
            {
                Organization = organization,
                PAT = pat,
                ConfigFile = configFile,
                Projects = projects,
                RepositoryName = repositoryName
            };

            (var isSuccess, var response) = await adapterCommunicator.CommunicateWithAdapterAsync(
                request,
                CommandTypes.GetEnvironmentsFromMultipleProjectsRequest,
                cancellationToken
                );

            if (!isSuccess)
            {
                return (AdapterStatus.Unknown, []);
            }

            var adapterResult = JsonSerializer.Deserialize<BaseResponse<Dictionary<string, object>>>(response)?.Data;

            if (adapterResult is null)
            {
                return (AdapterStatus.Unknown, []);
            }

            var isParseCompleted = int.TryParse(adapterResult["Status"].ToString(), out var i);

            if (!isParseCompleted)
            {
                return (AdapterStatus.Unknown, []);
            }

            var status = (AdapterStatus)i;
            var res = JsonSerializer.Deserialize<List<string>>(adapterResult["Data"].ToString() ?? "[]") ?? [];
            return (status, res);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error getting release environments for {repository} repository.", repositoryName);
            return (AdapterStatus.Unknown, []);
        }
    }
}
