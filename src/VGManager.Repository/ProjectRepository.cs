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
            var teamProjectReferences = await _projectHttpClient!.GetProjects();
            _projectHttpClient.Dispose();
            return GetResult(Status.Success, teamProjectReferences);
        } catch (VssUnauthorizedException)
        {
            return GetResult(Status.Unauthorized);
        }
        catch (VssServiceResponseException)
        {
            return GetResult(Status.ResourceNotFound);
        }
        catch (Exception) 
        {
            return GetResult(Status.Unknown);
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
            Console.WriteLine(ex);
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
