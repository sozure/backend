using VGManager.Services.Models;

namespace VGManager.Api.Handlers.GitVersion.Extensions;

public static class GitLatestTagsExtensions
{
    public static GitLatestTagsEntity ToEntity(this GitLatestTagsRequest req)
        => new()
        {
            RepositoryIds = req.RepositoryIds,
            PAT = req.PAT,
            Organization = req.Organization,
        };
}
