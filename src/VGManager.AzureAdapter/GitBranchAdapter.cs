using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using VGManager.AzureAdapter.Entities;
using VGManager.AzureAdapter.Interfaces;

namespace VGManager.AzureAdapter;

public class GitBranchAdapter: IGitBranchAdapter
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

    public async Task<(AdapterStatus, IEnumerable<TfvcBranch>)> GetAllAsync(
        string organization, 
        string pat,
        string project,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            _logger.LogInformation("Request git branches from {project} git project.", project);
            Setup(organization, pat);
            var client = await _connection.GetClientAsync<TfvcHttpClient>(cancellationToken);
            var branches = await client.GetBranchesAsync(project, cancellationToken: cancellationToken);
            return (AdapterStatus.Success, branches);
        } catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting git branches from {project} git project.", project);
            return (AdapterStatus.Unknown, Enumerable.Empty<TfvcBranch>());
        }
    }
}
