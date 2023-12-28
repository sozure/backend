using Microsoft.Extensions.Logging;
using VGManager.AzureAdapter.Entities;
using VGManager.AzureAdapter.Interfaces;
using VGManager.Services.Interfaces;
using VGManager.Services.Models.GitRepositories;

namespace VGManager.Services;

public class GitRepositoryService: IGitRepositoryService
{
    private readonly ILogger _logger;
    private readonly IGitRepositoryAdapter _gitRepositoryAdapter;

    public GitRepositoryService(ILogger<GitRepositoryService> logger, IGitRepositoryAdapter gitRepositoryAdapter)
    {
        _logger = logger;
        _gitRepositoryAdapter = gitRepositoryAdapter;
    }

    public async Task<GitRepositoryResult> GetAllAsync(
        string organization, 
        string project, 
        string pat, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            var repositoryNames = new List<string>();
            var repositories = await _gitRepositoryAdapter.GetAllAsync(organization, project, pat, cancellationToken);
            
            foreach (var repository in repositories)
            {
                repositoryNames.Add(repository.Name);
            }

            return new()
            {
                Repositories = repositoryNames,
                Status = AdapterStatus.Success
            };
        } catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting git repositories from {project} azure project.", project);
            return new()
            {
                Repositories = Enumerable.Empty<string>(),
                Status = AdapterStatus.Unknown
            };
        }
    }
}
