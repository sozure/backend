using Microsoft.Azure.Pipelines.WebApi;
using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.Organization.Client;
using Microsoft.VisualStudio.Services.WebApi;
using System.IO;
using System.Threading;
using VGManager.AzureAdapter.Entities;
using System.Linq;

namespace VGManager.AzureAdapter;

public class GitFileAdapter
{
    private VssConnection _connection = null!;
    private readonly ILogger _logger;

    public GitFileAdapter(ILogger<GitFileAdapter> logger)
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

    public async Task<(AdapterStatus, IEnumerable<string>)> GetFilePathAsync(
        string organization,
        string pat,
        string repositoryId,
        string fileName,
        string branch,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            _logger.LogInformation("Request file path from {project} git project.", repositoryId);
            Setup(organization, pat);
            return await GetFilePathAsync(branch, repositoryId, fileName, cancellationToken);
        }
        catch (ProjectDoesNotExistWithNameException ex)
        {
            _logger.LogError(ex, "{project} git project is not found.", repositoryId);
            return (AdapterStatus.ProjectDoesNotExist, Enumerable.Empty<string>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting file path from {project} git project.", repositoryId);
            return (AdapterStatus.Unknown, Enumerable.Empty<string>());
        }
    }

    private async Task<(AdapterStatus, IEnumerable<string>)> GetFilePathAsync(
        string version, 
        string repositoryId, 
        string fileName, 
        CancellationToken cancellationToken
        )
    {
        try
        {
            var client = await _connection.GetClientAsync<GitHttpClient>(cancellationToken);
            var request = new GitItemRequestData()
            {
                ItemDescriptors = new GitItemDescriptor[]
                {
                    new GitItemDescriptor()
                    {
                        RecursionLevel = VersionControlRecursionType.Full,
                        Version = version,
                        VersionType = GitVersionType.Branch,
                        Path = "/"
                    }
                }
            };
            var itemsBatch = await client.GetItemsBatchAsync(request, repositoryId, cancellationToken: cancellationToken);
            var result = new List<string>();
            foreach (var itemBatch in itemsBatch)
            {
                var elements = itemBatch.Where(item => item.Path.Contains(fileName)).ToList();
                if (elements.Any())
                {
                    result.Add(elements.FirstOrDefault()?.Path ?? string.Empty);
                }
            }
            return (AdapterStatus.Success, result);
        } catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting file path from {project} git project.", repositoryId);
            return (AdapterStatus.Unknown, Enumerable.Empty<string>());
        }
    }
}
