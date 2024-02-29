using Microsoft.TeamFoundation.Build.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using System.Text.Json;
using VGManager.Adapter.Models.Kafka;
using VGManager.Adapter.Models.Requests;
using VGManager.Adapter.Models.Response;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Services.Interfaces;

namespace VGManager.Services;

public class BuildPipelineService(
    IAdapterCommunicator adapterCommunicator
        ) : IBuildPipelineService
{
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

        (var isSuccess, var response) = await adapterCommunicator.CommunicateWithAdapterAsync(
            request,
            CommandTypes.GetBuildPipelineRequest,
            cancellationToken
            );

        if (!isSuccess)
        {
            return Guid.Empty;
        }

        var pipeline = JsonSerializer.Deserialize<BaseResponse<BuildDefinitionReference>>(response)?.Data;

        if (pipeline is null)
        {
            return Guid.Empty;
        }

        (isSuccess, response) = await adapterCommunicator.CommunicateWithAdapterAsync(
            request,
            CommandTypes.GetAllRepositoriesRequest,
            cancellationToken
            );

        if (!isSuccess)
        {
            return Guid.Empty;
        }

        var repositories = JsonSerializer.Deserialize<BaseResponse<IEnumerable<GitRepository>>>(response)?.Data;

        if (repositories is null)
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

        (var isSuccess, var response) = await adapterCommunicator.CommunicateWithAdapterAsync(
            request,
            CommandTypes.GetBuildPipelinesRequest,
            cancellationToken
            );

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

        (var isSuccess, var response) = await adapterCommunicator.CommunicateWithAdapterAsync(
            request,
            CommandTypes.RunBuildPipelineRequest,
            cancellationToken
            );

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

        (var isSuccess, var response) = await adapterCommunicator.CommunicateWithAdapterAsync(
            request,
            CommandTypes.RunBuildPipelinesRequest,
            cancellationToken
            );

        if (!isSuccess)
        {
            return AdapterStatus.Unknown;
        }

        var status = JsonSerializer.Deserialize<BaseResponse<AdapterStatus>>(response)?.Data;

        return status ?? AdapterStatus.Unknown;
    }
}
