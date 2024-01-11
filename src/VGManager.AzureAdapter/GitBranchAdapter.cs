using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using VGManager.AzureAdapter.Interfaces;
using VGManager.Models.StatusEnums;

namespace VGManager.AzureAdapter;

public class GitBranchAdapter : IGitBranchAdapter
{
    private readonly IHttpClientProvider _clientProvider;
    private readonly ILogger _logger;

    public GitBranchAdapter(IHttpClientProvider clientProvider, ILogger<GitBranchAdapter> logger)
    {
        _clientProvider = clientProvider;
        _logger = logger;
    }

    public async Task<(AdapterStatus, IEnumerable<string>)> GetAllAsync(
        string organization,
        string pat,
        string repositoryId,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            _clientProvider.Setup(organization, pat);
            _logger.LogInformation("Request git branches from {project} git project.", repositoryId);
            using var client = await _clientProvider.GetClientAsync<GitHttpClient>(cancellationToken);
            var branches = await client.GetBranchesAsync(repositoryId, cancellationToken: cancellationToken);

            return (AdapterStatus.Success, branches.Select(branch => branch.Name).ToList());
        }
        catch (ProjectDoesNotExistWithNameException ex)
        {
            _logger.LogError(ex, "{project} git project is not found.", repositoryId);
            return (AdapterStatus.ProjectDoesNotExist, Enumerable.Empty<string>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting git branches from {project} git project.", repositoryId);
            return (AdapterStatus.Unknown, Enumerable.Empty<string>());
        }
    }
}
