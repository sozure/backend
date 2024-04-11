using Microsoft.TeamFoundation.SourceControl.WebApi;
using System.Text.Json;
using VGManager.Adapter.Models.Kafka;
using VGManager.Adapter.Models.Models;
using VGManager.Adapter.Models.Requests;
using VGManager.Adapter.Models.Response;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Services.Interfaces;
using VGManager.Services.Models.GitRepositories;

namespace VGManager.Services;

public class GitRepositoryService(
    IAdapterCommunicator adapterCommunicator
        ) : IGitRepositoryService
{
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

        (var isSuccess, var response) = await adapterCommunicator.CommunicateWithAdapterAsync(
            request,
            CommandTypes.GetAllRepositoriesRequest,
            cancellationToken
            );

        if (!isSuccess)
        {
            return new AdapterResponseModel<IEnumerable<GitRepositoryResult>>
            {
                Data = Enumerable.Empty<GitRepositoryResult>(),
                Status = AdapterStatus.Unknown
            };
        }

        var rawResult = JsonSerializer.Deserialize<BaseResponse<IEnumerable<GitRepository>>>(response)?.Data;

        if (rawResult is null)
        {
            return new AdapterResponseModel<IEnumerable<GitRepositoryResult>>
            {
                Data = Enumerable.Empty<GitRepositoryResult>(),
                Status = AdapterStatus.Unknown
            };
        }

        var result = new List<GitRepositoryResult>();

        foreach (var res in rawResult)
        {
            result.Add(new()
            {
                RepositoryId = res.Id.ToString(),
                RepositoryName = res.Name,
                ProjectName = res.ProjectReference.Name
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

        (var isSuccess, var response) = await adapterCommunicator.CommunicateWithAdapterAsync(
            request,
            CommandTypes.GetVariablesFromConfigRequest,
            cancellationToken
            );

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
