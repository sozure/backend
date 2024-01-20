using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using System.Text.Json;
using VGManager.Adapter.Azure.Services.Requests;
using VGManager.Adapter.Client.Interfaces;
using VGManager.Adapter.Models.Kafka;
using VGManager.Adapter.Models.Response;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Services.Interfaces;

namespace VGManager.Services;

public class BuildPipelineService : IBuildPipelineService
{
    private readonly IVGManagerAdapterClientService _clientService;

    public BuildPipelineService(
        IVGManagerAdapterClientService clientService

        )
    {
        _clientService = clientService;
    }

    public async Task<Guid> GetRepositoryIdByBuildDefinitionAsync(
        string organization,
        string pat,
        string project,
        int id,
        CancellationToken cancellationToken = default
        )
    {
        var request = new GetBuildPipelineRequest()
        {
            Organization = organization,
            PAT = pat,
            Project = project,
            Id = id
        };

        (bool isSuccess, string response) = await _clientService.SendAndReceiveMessageAsync(
            CommandTypes.GetBuildPipelineRequest,
            JsonSerializer.Serialize(request),
            cancellationToken);

        if (!isSuccess)
        {
            return Guid.Empty;
        }

        var pipeline = JsonSerializer.Deserialize<BaseResponse<BuildDefinitionReference>>(response)?.Data;

        if (pipeline is null)
        {
            return Guid.Empty;
        }

        var repoRequest = new ExtendedBaseRequest()
        {
            Organization = organization,
            PAT = pat,
            Project = project
        };

        (isSuccess, response) = await _clientService.SendAndReceiveMessageAsync(
            CommandTypes.GetAllRepositoriesRequest,
            JsonSerializer.Serialize(repoRequest),
            cancellationToken);

        if (!isSuccess)
        {
            return Guid.Empty;
        }

        var repositories = JsonSerializer.Deserialize<BaseResponse<IEnumerable<GitRepository>>>(response)?.Data;

        if(repositories is null)
        {
            return Guid.Empty;
        }

        var repo = repositories.FirstOrDefault(r => r.Name == pipeline.Name);
        return repo?.Id ?? Guid.Empty;
    }

    public async Task<IEnumerable<Dictionary<string, string>>> GetBuildPipelinesAsync(
        string organization,
        string pat,
        string project,
        CancellationToken cancellationToken = default
        )
    {
        var result = new List<Dictionary<string, string>>();
        var request = new ExtendedBaseRequest()
        {
            Organization = organization,
            PAT = pat,
            Project = project
        };

        (bool isSuccess, string response) = await _clientService.SendAndReceiveMessageAsync(
            CommandTypes.GetBuildPipelinesRequest,
            JsonSerializer.Serialize(request),
            cancellationToken);

        if (!isSuccess)
        {
            return Enumerable.Empty<Dictionary<string, string>>();
        }

        var pipelines = JsonSerializer.Deserialize<BaseResponse<IEnumerable<BuildDefinitionReference>>>(response)?.Data;

        if (pipelines is null)
        {
            return Enumerable.Empty<Dictionary<string, string>>();
        }

        foreach (var pipeline in pipelines)
        {
            result.Add(
                new() { ["name"] = pipeline.Name, ["id"] = pipeline.Id.ToString() });
        }

        return result;
    }

    public async Task<AdapterStatus> RunBuildPipelineAsync(
        string organization,
        string pat,
        string project,
        int definitionId,
        string sourceBranch,
        CancellationToken cancellationToken = default
        )
    {
        var request = new RunBuildPipelineRequest()
        {
            Organization = organization,
            PAT = pat,
            Project = project,
            Id = definitionId,
            SourceBranch = sourceBranch
        };

        (bool isSuccess, string response) = await _clientService.SendAndReceiveMessageAsync(
            CommandTypes.RunBuildPipelineRequest,
            JsonSerializer.Serialize(request),
            cancellationToken);

        if (!isSuccess)
        {
            return AdapterStatus.Unknown;
        }

        var status = JsonSerializer.Deserialize<BaseResponse<AdapterStatus>>(response)?.Data;

        return status ?? AdapterStatus.Unknown;
    }

    public async Task<AdapterStatus> RunBuildPipelinesAsync(
        string organization,
        string pat,
        string project,
        IEnumerable<IDictionary<string, string>> pipelines,
        CancellationToken cancellationToken = default
        )
    {
        var request = new RunBuildPipelinesRequest()
        {
            Organization = organization,
            PAT = pat,
            Project = project,
            Pipelines = pipelines
        };

        (bool isSuccess, string response) = await _clientService.SendAndReceiveMessageAsync(
            CommandTypes.RunBuildPipelinesRequest,
            JsonSerializer.Serialize(request),
            cancellationToken);

        if (!isSuccess)
        {
            return AdapterStatus.Unknown;
        }

        var status = JsonSerializer.Deserialize<BaseResponse<AdapterStatus>>(response)?.Data;

        return status ?? AdapterStatus.Unknown;
    }
}
