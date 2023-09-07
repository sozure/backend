using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using VGManager.AzureAdapter.Entities;
using VGManager.AzureAdapter.Interfaces;

namespace VGManager.AzureAdapter;
public class ProjectAdapter : IProjectAdapter
{
    private ProjectHttpClient? _projectHttpClient;
    private readonly ILogger _logger;

    public ProjectAdapter(ILogger<ProjectAdapter> logger)
    {
        _logger = logger;
    }

    public async Task<ProjectEntity> GetProjects(string baseUrl, string pat, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Get projects from {baseUrl}.", baseUrl);
            await GetConnectionAsync(baseUrl, pat);
            var teamProjectReferences = await _projectHttpClient!.GetProjects();
            _projectHttpClient.Dispose();
            return GetResult(Status.Success, teamProjectReferences);
        }
        catch (VssUnauthorizedException ex)
        {
            var status = Status.Unauthorized;
            _logger.LogError(ex, "Couldn't get projects. Status: {status}.", status);
            return GetResult(status);
        }
        catch (VssServiceResponseException ex)
        {
            var status = Status.ResourceNotFound;
            _logger.LogError(ex, "Couldn't get projects. Status: {status}.", status);
            return GetResult(status);
        }
        catch (Exception ex)
        {
            var status = Status.Unknown;
            _logger.LogError(ex, "Couldn't get projects. Status: {status}.", status);
            return GetResult(status);
        }
    }

    private async Task GetConnectionAsync(string baseUrl, string pat)
    {
        Uri.TryCreate(baseUrl, UriKind.Absolute, out Uri? uri);
        try
        {
            var credentials = new VssBasicCredential(string.Empty, pat);
            var connection = new VssConnection(uri, credentials);
            await connection.ConnectAsync(VssConnectMode.Profile, default);
            _projectHttpClient = new ProjectHttpClient(uri, credentials);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Couldn't establish connection.");
            throw;
        }
    }

    private static ProjectEntity GetResult(Status status, IEnumerable<TeamProjectReference> projects)
    {
        return new()
        {
            Status = status,
            Projects = projects
        };
    }

    private static ProjectEntity GetResult(Status status)
    {
        return new()
        {
            Status = status,
            Projects = Enumerable.Empty<TeamProjectReference>()
        };
    }
}
