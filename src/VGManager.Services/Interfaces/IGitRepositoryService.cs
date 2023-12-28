using VGManager.Services.Models.GitRepositories;

namespace VGManager.Services.Interfaces;

public interface IGitRepositoryService
{
    Task<GitRepositoryResult> GetAllAsync(
        string organization,
        string project,
        string pat,
        CancellationToken cancellationToken = default);
}
