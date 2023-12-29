using VGManager.Services.Models.GitRepositories;

namespace VGManager.Services.Interfaces;

public interface IGitRepositoryService
{
    Task<GitRepositoryResults> GetAllAsync(
        string organization,
        string project,
        string pat,
        CancellationToken cancellationToken = default);

    Task<GitRepositoryVariablesResult> GetVariablesFromConfigAsync(
        GitRepositoryModel gitRepositoryModel,
        CancellationToken cancellationToken = default
        );
}
