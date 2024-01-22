using Microsoft.TeamFoundation.SourceControl.WebApi;
using System.Text.Json;
using VGManager.Adapter.Azure.Services.Requests;
using VGManager.Adapter.Client.Interfaces;
using VGManager.Adapter.Models.Kafka;
using VGManager.Adapter.Models.Models;
using VGManager.Adapter.Models.Requests;
using VGManager.Adapter.Models.Response;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Services.Interfaces;
using VGManager.Services.Models.GitRepositories;

namespace VGManager.Services;

public class GitRepositoryService : IGitRepositoryService
{
    private readonly IVGManagerAdapterClientService _clientService;

    public GitRepositoryService(
        IVGManagerAdapterClientService clientService
        )
    {
        _clientService = clientService;
    }

    public async Task<AdapterResponseModel<IEnumerable<GitRepositoryResult>>> GetAllAsync(
        string organization,
        string project,
        string pat,
        CancellationToken cancellationToken = default)
    {
        var request = new ExtendedBaseRequest
        {
            Organization = organization,
            PAT = pat,
            Project = project
        };

        (bool isSuccess, string response) = await _clientService.SendAndReceiveMessageAsync(
            CommandTypes.GetAllRepositoriesRequest,
            JsonSerializer.Serialize(request),
            cancellationToken);

        if (!isSuccess)
        {
            return new AdapterResponseModel<IEnumerable<GitRepositoryResult>>
            {
                Data = Enumerable.Empty<GitRepositoryResult>()
            };
        }

        var rawResult = JsonSerializer.Deserialize<BaseResponse<IEnumerable<GitRepository>>>(response)?.Data;

        if (rawResult is null)
        {
            return new AdapterResponseModel<IEnumerable<GitRepositoryResult>>
            {
                Data = Enumerable.Empty<GitRepositoryResult>()
            };
        }

        var result = new List<GitRepositoryResult>();

        foreach (var res in rawResult)
        {
            result.Add(new()
            {
                RepositoryId = res.Id.ToString(),
                RepositoryName = res.Name
            });
        }

        return new AdapterResponseModel<IEnumerable<GitRepositoryResult>>
        {
            Status = AdapterStatus.Success,
            Data = result
        };
    }

    public async Task<AdapterResponseModel<IEnumerable<string>>> GetVariablesFromConfigAsync(
        GitRepositoryModel gitRepositoryModel,
        CancellationToken cancellationToken = default
        )
    {
        var request = new GitRepositoryRequest<string>
        {
            Organization = gitRepositoryModel.Organization,
            PAT = gitRepositoryModel.PAT,
            Project = gitRepositoryModel.Project,
            Branch = gitRepositoryModel.Branch,
            Delimiter = gitRepositoryModel.Delimiter,
            Exceptions = gitRepositoryModel.Exceptions,
            FilePath = gitRepositoryModel.FilePath,
            RepositoryId = gitRepositoryModel.RepositoryId
        };

        (bool isSuccess, string response) = await _clientService.SendAndReceiveMessageAsync(
            CommandTypes.GetVariablesFromConfigRequest,
            JsonSerializer.Serialize(request),
            cancellationToken);

        if (!isSuccess)
        {
            return new AdapterResponseModel<IEnumerable<string>>
            {
                Status = AdapterStatus.Unknown,
                Data = Enumerable.Empty<string>()
            };
        }

        var result = JsonSerializer.Deserialize<BaseResponse<List<string>>>(response)?.Data;

        if (result is null)
        {
            return new AdapterResponseModel<IEnumerable<string>>
            {
                Status = AdapterStatus.Unknown,
                Data = Enumerable.Empty<string>()
            };
        }

        return new AdapterResponseModel<IEnumerable<string>>
        {
            Status = AdapterStatus.Success,
            Data = result
        };
    }
}
