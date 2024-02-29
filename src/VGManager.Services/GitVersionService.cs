using System.Text.Json;
using VGManager.Adapter.Models.Kafka;
using VGManager.Adapter.Models.Requests;
using VGManager.Adapter.Models.Response;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Services.Interfaces;
using VGManager.Services.Models;

namespace VGManager.Services;

public class GitVersionService(
    IGitAdapterCommunicatorService gitAdapterCommunicatorService,
    IAdapterCommunicator adapterCommunicator
        ) : IGitVersionService
{
    public async Task<(AdapterStatus, IEnumerable<string>)> GetBranchesAsync(
        string organization,
        string pat,
        string repositoryId,
        CancellationToken cancellationToken = default
        )
    {
        var commandType = CommandTypes.GetBranchesRequest;
        return await GetInformationAsync(commandType, organization, pat, repositoryId, cancellationToken);
    }

    public async Task<(AdapterStatus, IEnumerable<string>)> GetTagsAsync(
        string organization,
        string pat,
        Guid repositoryId,
        CancellationToken cancellationToken = default
        )
    {
        var commandType = CommandTypes.GetTagsRequest;
        return await GetInformationAsync(commandType, organization, pat, repositoryId, cancellationToken);
    }

    public async Task<(AdapterStatus, string)> CreateTagAsync(
        CreateTagEntity tagEntity,
        CancellationToken cancellationToken = default
        )
    {
        var repositoryId = tagEntity.RepositoryId;
        var organization = tagEntity.Organization;
        var pat = tagEntity.PAT;
        (var branchesStatus, var branches) = await GetBranchesAsync(organization, pat, repositoryId.ToString(), cancellationToken);
        (var tagsStatus, var tags) = await GetTagsAsync(organization, pat, repositoryId, cancellationToken);

        if (branchesStatus == AdapterStatus.Success && tagsStatus == AdapterStatus.Success)
        {
            return await CreateTagAsync(tagEntity, tags, branches, cancellationToken);
        }

        return (AdapterStatus.Unknown, string.Empty);
    }

    private async Task<(AdapterStatus, string)> CreateTagAsync(
        CreateTagEntity tagEntity,
        IEnumerable<string> tags,
        IEnumerable<string> branches,
        CancellationToken cancellationToken
        )
    {
        var lastTag = tags.LastOrDefault() ?? "refs/tags/0.0.0";
        var defaultBranch = branches.FirstOrDefault(branch => branch.Equals("main") || branch.Equals("master"));
        if (!string.IsNullOrEmpty(defaultBranch))
        {
            var lastTagVersion = lastTag.Replace("refs/tags/", string.Empty);
            var tagElements = lastTagVersion.Split('.');
            var newTag = GetNewVersion(tagEntity.TagName, tagElements);

            if (string.IsNullOrEmpty(newTag))
            {
                return (AdapterStatus.Unknown, string.Empty);
            }

            tagEntity.TagName = newTag;

            var request = new CreateTagRequest()
            {
                Organization = tagEntity.Organization,
                PAT = tagEntity.PAT,
                RepositoryId = tagEntity.RepositoryId,
                DefaultBranch = defaultBranch,
                Project = tagEntity.Project,
                TagName = newTag,
                UserName = tagEntity.UserName,
                Description = tagEntity.Description
            };

            (var isSuccess, var response) = await adapterCommunicator.CommunicateWithAdapterAsync(
                request,
                CommandTypes.CreateTagRequest,
                cancellationToken
                );

            if (!isSuccess)
            {
                return (AdapterStatus.Unknown, string.Empty);
            }

            var result = JsonSerializer.Deserialize<BaseResponse<Dictionary<string, object>>>(response)?.Data;

            if (result is null)
            {
                return (AdapterStatus.Unknown, string.Empty);
            }

            var isParseCompleted = int.TryParse(result["Status"].ToString(), out var i);

            if (!isParseCompleted)
            {
                return (AdapterStatus.Unknown, string.Empty);
            }

            var status = (AdapterStatus)i;
            var res = result["Data"].ToString() ?? string.Empty;
            return (status, res);

        }
        return (AdapterStatus.Unknown, string.Empty);
    }

    private static string GetNewVersion(string tagName, string[] tagElements)
    {
        string newTag;
        switch (tagName)
        {
            case "major":
                newTag = $"{int.Parse(tagElements[0]) + 1}.0.0";
                return newTag;
            case "minor":
                newTag = $"{tagElements[0]}.{int.Parse(tagElements[1]) + 1}.0";
                return newTag;
            case "patch":
                newTag = $"{tagElements[0]}.{tagElements[1]}.{int.Parse(tagElements[2]) + 1}";
                return newTag;
            default:
                return string.Empty;
        }
    }

    private async Task<(AdapterStatus, IEnumerable<string>)> GetInformationAsync<T>(
        string commandType,
        string organization,
        string pat,
        T repositoryId,
        CancellationToken cancellationToken
        )
    {
        var request = new GitFileBaseRequest<T>()
        {
            Organization = organization,
            PAT = pat,
            RepositoryId = repositoryId
        };

        return await gitAdapterCommunicatorService.GetInformationAsync(commandType, request, cancellationToken);
    }
}
