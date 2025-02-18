using VGManager.Api.Handlers.GitRepository.Request;
using VGManager.Services.Models.GitRepositories;

namespace VGManager.Api.Handlers.GitRepository.Extensions;

public static class GitRepositoryExtensions
{
    public static GitRepositoryModel ToModel(this GitRepositoryVariablesRequest request)
        => new()
        {
            Branch = request.Branch,
            Delimiter = request.Delimiter,
            Exceptions = request.Exceptions,
            FilePath = request.FilePath,
            Organization = request.Organization,
            PAT = request.PAT,
            Project = request.Project,
            RepositoryId = request.RepositoryId
        };
}
