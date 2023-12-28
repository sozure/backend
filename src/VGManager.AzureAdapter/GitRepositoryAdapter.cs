using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
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

    public async Task<IEnumerable<string>> GetVariablesFromConfigAsync(
        string organization,
        string project,
        string pat,
        string gitRepositoryId,
        string filePath,
        string delimiter,
        CancellationToken cancellationToken = default
        )
    {
        _logger.LogInformation(
            "Requesting configurations from {project} azure project, {gitRepositoryId} git repository.", 
            project, 
            gitRepositoryId
            );
        Setup(organization, pat);
        var gitClient = await _connection.GetClientAsync<GitHttpClient>(cancellationToken);
        var item = await gitClient.GetItemTextAsync(
            project: project, 
            repositoryId: gitRepositoryId, 
            path: filePath, 
            cancellationToken: cancellationToken
            );
        var json = await GetJsonObjectAsync(item, cancellationToken);
        var result = GetJsonKeys(json, delimiter);
        return result;
    }

    public static List<string> GetJsonKeys(JsonElement jsonObject, string delimiter)
    {
        var keys = new List<string>();
        GetJsonKeysHelper(jsonObject, delimiter, string.Empty, keys);
        return keys;
    }

    private static void GetJsonKeysHelper(JsonElement jsonObject, string delimiter, string prefix, List<string> keys)
    {
        if (jsonObject.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in jsonObject.EnumerateObject())
            {
                var name = property.Name;
                var key = prefix + name + delimiter;

                if (property.Value.ValueKind != JsonValueKind.Object)
                {
                    keys.Add(key.Remove(key.Length - delimiter.Length));
                }

                GetJsonKeysHelper(property.Value, delimiter, key, keys);
            }
        }
    }

    private static async Task<JsonElement> GetJsonObjectAsync(Stream item, CancellationToken cancellationToken)
    {
        using var stream = new MemoryStream();
        byte[] buffer = new byte[2048]; // read in chunks of 2KB
        int bytesRead;
        while ((bytesRead = await item.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
        {
            await stream.WriteAsync(buffer, 0, bytesRead, cancellationToken);
        }
        var result = stream.ToArray();
        var itemText = Encoding.UTF8.GetString(result);
        var json = JsonSerializer.Deserialize<JsonElement>(itemText);
        return json;
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
