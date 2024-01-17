using VGManager.AzureAdapter.Entities;
using VGManager.AzureAdapter.Interfaces;
using VGManager.Models.StatusEnums;
using VGManager.Services.Interfaces;

namespace VGManager.Services;

public class GitVersionService : IGitVersionService
{
    private readonly IGitVersionAdapter _gitBranchAdapter;
    private readonly ISprintAdapter _sprintAdapter;

    public GitVersionService(IGitVersionAdapter gitBranchAdapter, ISprintAdapter sprintAdapter)
    {
        _gitBranchAdapter = gitBranchAdapter;
        _sprintAdapter = sprintAdapter;
    }

    public async Task<(AdapterStatus, IEnumerable<string>)> GetBranchesAsync(
        string organization,
        string pat,
        string repositoryId,
        CancellationToken cancellationToken = default
        )
    {
        return await _gitBranchAdapter.GetBranchesAsync(organization, pat, repositoryId, cancellationToken);
    }

    public async Task<(AdapterStatus, IEnumerable<string>)> GetTagsAsync(
        string organization,
        string pat,
        Guid repositoryId,
        CancellationToken cancellationToken = default
        )
    {
        return await _gitBranchAdapter.GetTagsAsync(organization, pat, repositoryId, cancellationToken);
    }

    public async Task<(AdapterStatus, string)> CreateTagAsync(
        CreateTagEntity tagEntity,
        CancellationToken cancellationToken = default
        )
    {
        (var tagStatus, var tags) = await _gitBranchAdapter.GetTagsAsync(
            tagEntity.Organization, 
            tagEntity.PAT, 
            tagEntity.RepositoryId, 
            cancellationToken
            );

        (var branchStatus, var branches) = await _gitBranchAdapter.GetBranchesAsync(
            tagEntity.Organization,
            tagEntity.PAT,
            tagEntity.RepositoryId.ToString(),
            cancellationToken
            );

        (var sprintStatus, var sprint) = await _sprintAdapter.GetCurrentSprintAsync(tagEntity.Project, cancellationToken);

        if (tagStatus == AdapterStatus.Success && branchStatus == AdapterStatus.Success && sprintStatus == AdapterStatus.Success)
        {
            return await CreateTagAsync(tagEntity, tags, branches, sprint, cancellationToken);
        }

        return (tagStatus, string.Empty);
    }

    private async Task<(AdapterStatus, string)> CreateTagAsync(
        CreateTagEntity tagEntity, 
        IEnumerable<string> tags, 
        IEnumerable<string> branches, 
        string sprint, 
        CancellationToken cancellationToken
        )
    {
        var lastTag = tags.LastOrDefault();
        var defaultBranch = branches.FirstOrDefault(branch => branch.Equals("main") || branch.Equals("master"));
        if (lastTag is not null && !string.IsNullOrEmpty(defaultBranch))
        {
            var lastTagVersion = lastTag.Replace("refs/tags/", string.Empty);
            var tagElements = lastTagVersion.Split('.');
            var newTag = GetNewVersion(tagEntity.TagName, tagElements);

            if (string.IsNullOrEmpty(newTag))
            {
                return (AdapterStatus.Unknown, string.Empty);
            }

            tagEntity.TagName = newTag;
            return await _gitBranchAdapter.CreateTagAsync(tagEntity, defaultBranch, sprint, cancellationToken);
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
