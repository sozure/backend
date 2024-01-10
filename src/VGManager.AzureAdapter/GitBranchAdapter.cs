using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using VGManager.AzureAdapter.Interfaces;
using VGManager.Models.StatusEnums;

namespace VGManager.AzureAdapter;

public class GitBranchAdapter : IGitBranchAdapter
{
    private VssConnection _connection = null!;
    private readonly ILogger _logger;

    public GitBranchAdapter(ILogger<GitBranchAdapter> logger)
    {
        _logger = logger;
    }

    public void Setup(string organization, string pat)
    {
        var uriString = $"https://dev.azure.com/{organization}";
        Uri uri;
        Uri.TryCreate(uriString, UriKind.Absolute, out uri!);

        var credentials = new VssBasicCredential(string.Empty, pat);
        _connection = new VssConnection(uri, credentials);
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
            _logger.LogInformation("Request git branches from {project} git project.", repositoryId);
            Setup(organization, pat);
            var client = await _connection.GetClientAsync<GitHttpClient>(cancellationToken);
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
