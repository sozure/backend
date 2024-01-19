using VGManager.Adapter.Models.Models;
using VGManager.Services.Models.GitRepositories;

namespace VGManager.Services.Interfaces;

public interface IGitRepositoryService
{
    Task<AdapterResponseModel<IEnumerable<GitRepositoryResult>>> GetAllAsync(
        string organization,
        string project,
        string pat,
        CancellationToken cancellationToken = default);

    Task<AdapterResponseModel<IEnumerable<string>>> GetVariablesFromConfigAsync(
        GitRepositoryModel gitRepositoryModel,
        CancellationToken cancellationToken = default
        );
}
