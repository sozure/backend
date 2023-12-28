using VGManager.Services.Models.GitRepositories;

namespace VGManager.Services.Interfaces;

public interface IGitRepositoryService
{
    Task<GitRepositoryResults> GetAllAsync(
        string organization,
        string project,
        string pat,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<string>> GetVariablesFromConfigAsync(
        string organization,
        string project,
        string pat,
        string gitRepositoryId,
        string filePath,
        string delimiter,
        CancellationToken cancellationToken = default
        );
}
