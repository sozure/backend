using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using VGManager.Repository.Interfaces;

namespace VGManager.Repository;
public class ProjectRepository : IProjectRepository
{
    private ProjectHttpClient? _projectHttpClient;

    public async Task<IEnumerable<TeamProjectReference>> GetProjects(string baseUrl, string pat, CancellationToken cancellationToken = default)
    {
        await GetConnectionAsync(baseUrl, pat);
        var teamProjectReferences = await _projectHttpClient!.GetProjects();
        _projectHttpClient.Dispose();
        return teamProjectReferences.ToList();
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
