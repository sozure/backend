using System.Text.Json;
using VGManager.Adapter.Azure.Services.Requests;
using VGManager.Adapter.Models.Kafka;
using VGManager.Adapter.Models.Requests;
using VGManager.Adapter.Models.Response;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Services.Interfaces;
using VGManager.Services.Models;

namespace VGManager.Services;

public class GitVersionService : IGitVersionService
{
    private readonly IAdapterCommunicator _adapterCommunicator;

    public GitVersionService(
        IAdapterCommunicator adapterCommunicator
        )
    {
        _adapterCommunicator = adapterCommunicator;
    }

    public async Task<(AdapterStatus, IEnumerable<string>)> GetBranchesAsync(
        string organization,
        string pat,
        string repositoryId,
        CancellationToken cancellationToken = default
        )
    {
        var request = new GitFileBaseRequest<string>()
        {
            Organization = organization,
            PAT = pat,
            RepositoryId = repositoryId,
        };

        (var isSuccess, var response) = await _adapterCommunicator.CommunicateWithAdapterAsync(
            request,
            CommandTypes.GetBranchesRequest,
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

        int.TryParse(result["Status"].ToString(), out var i);
        var status = (AdapterStatus)i;
        var res = JsonSerializer.Deserialize<List<string>>(result["Data"].ToString() ?? "[]") ?? [];
        return (status, res);
    }

    public async Task<(AdapterStatus, IEnumerable<string>)> GetTagsAsync(
        string organization,
        string pat,
        Guid repositoryId,
        CancellationToken cancellationToken = default
        )
    {
        var request = new GitFileBaseRequest<Guid>()
        {
            Organization = organization,
            PAT = pat,
            RepositoryId = repositoryId,
        };

        (var isSuccess, var response) = await _adapterCommunicator.CommunicateWithAdapterAsync(
            request,
            CommandTypes.GetTagsRequest,
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

        int.TryParse(result["Status"].ToString(), out var i);
        var status = (AdapterStatus)i;
        var res = JsonSerializer.Deserialize<List<string>>(result["Data"].ToString() ?? "[]") ?? [];
        return (status, res);
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
                UserName = tagEntity.UserName
            };

            (var isSuccess, var response) = await _adapterCommunicator.CommunicateWithAdapterAsync(
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

            int.TryParse(result["Status"].ToString(), out var i);
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
}
