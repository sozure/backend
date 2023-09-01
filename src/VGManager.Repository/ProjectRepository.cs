using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using VGManager.Repository.Entities;
using VGManager.Repository.Interfaces;

namespace VGManager.Repository;
public class ProjectRepository : IProjectRepository
{
    private ProjectHttpClient? _projectHttpClient;

    public async Task<ProjectEntity> GetProjects(string baseUrl, string pat, CancellationToken cancellationToken = default)
    {
        try
        {
            await GetConnectionAsync(baseUrl, pat);
        } catch (VssUnauthorizedException)
        {
            return new()
            {
                Status = Status.Unauthorized,
                Projects = Enumerable.Empty<TeamProjectReference>()
            };
        }
        catch (VssServiceResponseException)
        {
            return new()
            {
                Status = Status.ResourceNotFound,
                Projects = Enumerable.Empty<TeamProjectReference>()
            };
        }

        var teamProjectReferences = await _projectHttpClient!.GetProjects();
        _projectHttpClient.Dispose();
        
        return new()
        {
            Status = Status.Success,
            Projects = teamProjectReferences
        };
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
            Console.WriteLine(ex);
            throw;
        }
    }
}
