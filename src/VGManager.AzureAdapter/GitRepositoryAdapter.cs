using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.SourceControl.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.WebApi;
using System.Text;
using System.Text.Json;
using VGManager.AzureAdapter.Entities;
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

    public async Task<List<string>> GetVariablesFromConfigAsync(
        GitRepositoryEntity gitRepositoryEntity,
        CancellationToken cancellationToken = default
        )
    {
        var project = gitRepositoryEntity.Project;
        var gitRepositoryId = gitRepositoryEntity.GitRepositoryId;

        _logger.LogInformation(
            "Requesting configurations from {project} azure project, {gitRepositoryId} git repository.",
            project,
            gitRepositoryId
            );

        var gitClient = await _connection.GetClientAsync<GitHttpClient>(cancellationToken);
        var gitVersionDescriptor = new GitVersionDescriptor
        {
            VersionType = GitVersionType.Branch, 
            Version = "develop" 
        };

        var item = await gitClient.GetItemTextAsync(
            project: project,
            repositoryId: gitRepositoryId,
            path: gitRepositoryEntity.FilePath,
            versionDescriptor: gitVersionDescriptor,
            cancellationToken: cancellationToken
            );
        
        var json = await GetJsonObjectAsync(item, cancellationToken);
        var result = GetJsonKeys(json, gitRepositoryEntity.Exceptions ?? Enumerable.Empty<string>(), gitRepositoryEntity.Delimiter);
        return result;
    }

    private static List<string> GetJsonKeys(JsonElement jsonObject, IEnumerable<string> exceptions, string delimiter)
    {
        var keys = new List<string>();
        GetJsonKeysHelper(jsonObject, delimiter, string.Empty, exceptions, keys);
        return keys;
    }

    private static void GetJsonKeysHelper(
        JsonElement jsonObject, 
        string delimiter, 
        string prefix, 
        IEnumerable<string> exceptions, 
        List<string> keys
        )
    {
        if (jsonObject.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in jsonObject.EnumerateObject())
            {
                var name = property.Name;
                var key = prefix + name + delimiter;

                if (property.Value.ValueKind != JsonValueKind.Object || exceptions.Contains(property.Name))
                {
                    keys.Add(key.Remove(key.Length - delimiter.Length));
                }

                if (!exceptions.Contains(property.Name))
                {
                    GetJsonKeysHelper(property.Value, delimiter, key, exceptions, keys);
                }
            }
        }
    }

    private static async Task<JsonElement> GetJsonObjectAsync(Stream item, CancellationToken cancellationToken)
    {
        using var stream = new MemoryStream();
        var buffer = new byte[2048];
        int bytesRead;
        while ((bytesRead = await item.ReadAsync(buffer, cancellationToken)) > 0)
        {
            await stream.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken);
        }
        var result = stream.ToArray();
        var itemText = Encoding.UTF8.GetString(result);
        return JsonSerializer.Deserialize<JsonElement>(itemText);
    }

    public void Setup(string organization, string pat)
    {
        var uriString = $"https://dev.azure.com/{organization}";
        Uri uri;
        Uri.TryCreate(uriString, UriKind.Absolute, out uri!);

        var credentials = new VssBasicCredential(string.Empty, pat);
        _connection = new VssConnection(uri, credentials);
    }
}
