using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using VGManager.AzureAdapter.Interfaces;

namespace VGManager.AzureAdapter;

public class GitRepositoryAdapter: IGitRepositoryAdapter
{
    private VssConnection _connection = null!;
    private readonly ILogger _logger;

    public GitRepositoryAdapter(ILogger<GitRepositoryAdapter> logger)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<GitRepository>> GetAllAsync(
        string organization, 
        string project, 
        string pat, 
        CancellationToken cancellationToken = default
        )
    {
        _logger.LogInformation("Request git repositories from {project} azure project.", project);
        Setup(organization, pat);
        var gitClient = await _connection.GetClientAsync<GitHttpClient>(cancellationToken);
        var repositories = await gitClient.GetRepositoriesAsync(cancellationToken: cancellationToken);
        return repositories.Where(repo => (!repo.IsDisabled ?? false) && repo.ProjectReference.Name == project).ToList();
    }

    private void Setup(string organization, string pat)
    {
        var uriString = $"https://dev.azure.com/{organization}";
        Uri uri;
        Uri.TryCreate(uriString, UriKind.Absolute, out uri!);

        var credentials = new VssBasicCredential(string.Empty, pat);
        _connection = new VssConnection(uri, credentials);
    }
}
