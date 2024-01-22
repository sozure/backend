using Microsoft.Extensions.Logging;
using System.Text.Json;
using VGManager.Adapter.Azure.Services.Requests;
using VGManager.Adapter.Client.Interfaces;
using VGManager.Adapter.Models.Kafka;
using VGManager.Adapter.Models.Response;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Services.Interfaces;

namespace VGManager.Services;

public class ReleasePipelineService : IReleasePipelineService
{
    private readonly IVGManagerAdapterClientService _clientService;

    private readonly ILogger _logger;

    public ReleasePipelineService(
        IVGManagerAdapterClientService clientService,
        ILogger<ReleasePipelineService> logger
        )
    {
        _clientService = clientService;
        _logger = logger;
    }

    public async Task<(AdapterStatus, IEnumerable<string>)> GetProjectsWhichHaveCorrespondingReleasePipelineAsync(
        string organization,
        string pat,
        IEnumerable<string> projects,
        string repositoryName,
        string configFile,
        CancellationToken cancellationToken = default
        )
    {
        var (status, result) = (AdapterStatus.Unknown, new List<string>());
        try
        {
            foreach (var project in projects)
            {
                var adapterResult = await GetEnvironmentsAsync(organization, pat, project, repositoryName, configFile, cancellationToken);
                var subStatus = adapterResult.Item1;
                var subResult = adapterResult.Item2;

                if (subStatus == AdapterStatus.Success && subResult.Any())
                {
                    result.Add(project);
                    status = AdapterStatus.Success;
                }
            }
            return (status, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting projects with release pipelines for {repository} repository.", repositoryName);
            return (AdapterStatus.Unknown, Enumerable.Empty<string>());
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
            _logger.LogInformation("Request release environments for {repository} repository.", repositoryName);
            var request = new ReleasePipelineRequest()
            {
                Organization = organization,
                PAT = pat,
                ConfigFile = configFile,
                Project = project,
                RepositoryName = repositoryName
            };

            (bool isSuccess, string response) = await _clientService.SendAndReceiveMessageAsync(
                CommandTypes.GetEnvironmentsRequest,
                JsonSerializer.Serialize(request),
                cancellationToken);

            if (!isSuccess)
            {
                return (AdapterStatus.Unknown, Enumerable.Empty<string>());
            }

            var adapterResult = JsonSerializer.Deserialize<BaseResponse<Dictionary<string, object>>>(response)?.Data;

            if (adapterResult is null)
            {
                return (AdapterStatus.Unknown, Enumerable.Empty<string>());
            }

            int.TryParse(adapterResult["Status"].ToString(), out var i);
            var status = (AdapterStatus)i;
            var res = JsonSerializer.Deserialize<List<string>>(adapterResult["Data"].ToString() ?? "[]") ?? [];
            return (status, res);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting release environments for {repository} repository.", repositoryName);
            return (AdapterStatus.Unknown, Enumerable.Empty<string>());
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
            _logger.LogInformation("Request variable groups connected to release pipeline for {repository} repository.", repositoryName);
            var request = new ReleasePipelineRequest()
            {
                Organization = organization,
                PAT = pat,
                ConfigFile = configFile,
                Project = project,
                RepositoryName = repositoryName
            };

            (bool isSuccess, string response) = await _clientService.SendAndReceiveMessageAsync(
                CommandTypes.GetVariableGroupsRequest,
                JsonSerializer.Serialize(request),
                cancellationToken);

            if (!isSuccess)
            {
                return (AdapterStatus.Unknown, Enumerable.Empty<(string, string)>());
            }

            var adapterResult = JsonSerializer.Deserialize<BaseResponse<Dictionary<string, object>>>(response)?.Data;

            if (adapterResult is null)
            {
                return (AdapterStatus.Unknown, Enumerable.Empty<(string, string)>());
            }

            int.TryParse(adapterResult["Status"].ToString(), out var i);
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
            _logger.LogError(ex, "Error getting variable groups connected to release pipeline for {repository} repository.", repositoryName);
            return (AdapterStatus.Unknown, Enumerable.Empty<(string, string)>());
        }
    }
}
