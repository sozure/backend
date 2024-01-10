using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.Core.WebApi;
using Microsoft.TeamFoundation.DistributedTask.WebApi;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi;
using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Clients;
using Microsoft.VisualStudio.Services.ReleaseManagement.WebApi.Contracts;
using Microsoft.VisualStudio.Services.WebApi;
using VGManager.AzureAdapter.Interfaces;
using VGManager.Models.StatusEnums;

namespace VGManager.AzureAdapter;

public class ReleasePipelineAdapter : IReleasePipelineAdapter
{
    private VssConnection _connection = null!;
    private readonly ILogger _logger;

    private readonly string[] Replacable = { "Deploy to ", "Transfer to " };

    private readonly string[] ExcludableEnvironments = { "OTP container registry" };

    public ReleasePipelineAdapter(ILogger<ReleasePipelineAdapter> logger)
    {
        _logger = logger;
    }

    public async Task<(AdapterStatus, IEnumerable<string>)> GetEnvironmentsAsync(
        string organization,
        string pat,
        string project,
        string repositoryName,
        string configFile,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            _logger.LogInformation("Request environments for {repository} git repository from {project} azure project.", repositoryName, project);
            var definition = await GetReleaseDefinitionAsync(organization, pat, project, repositoryName, configFile, cancellationToken);
            var rawResult = definition?.Environments.Select(env => env.Name).ToList() ?? Enumerable.Empty<string>();
            var result = new List<string>();

            foreach (var rawElement in rawResult)
            {
                var element = Replacable.Where(rawElement.Contains).Select(replace => rawElement.Replace(replace, string.Empty));
                if (!element.Any())
                {
                    element = new[] { rawElement };
                }
                result.AddRange(element.Where(element => !ExcludableEnvironments.Contains(element)));
            }

            return (definition is null ? AdapterStatus.Unknown : AdapterStatus.Success, result);
        }
        catch (ProjectDoesNotExistWithNameException ex)
        {
            _logger.LogError(ex, "{project} azure project is not found.", project);
            return (AdapterStatus.ProjectDoesNotExist, Enumerable.Empty<string>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting git branches from {project} azure project.", project);
            return (AdapterStatus.Unknown, Enumerable.Empty<string>());
        }
    }

    public async Task<(AdapterStatus, IEnumerable<(string, string)>)> GetVariableGroupsAsync(
        string organization,
        string pat,
        string project,
        string repositoryName,
        string configFile,
        CancellationToken cancellationToken = default
        )
    {
        try
        {
            _logger.LogInformation(
                "Request corresponding variable groups for {repository} repository, {project} azure project.",
                repositoryName,
                project
                );
            var definition = await GetReleaseDefinitionAsync(organization, pat, project, repositoryName, configFile, cancellationToken);

            if (definition is null)
            {
                return (AdapterStatus.Unknown, Enumerable.Empty<(string, string)>());
            }

            var variableGroups = await GetVariableGroupNames(project, definition, cancellationToken);

            return (AdapterStatus.Success, variableGroups);
        }
        catch (ProjectDoesNotExistWithNameException ex)
        {
            _logger.LogError(ex, "{project} azure project is not found.", project);
            return (AdapterStatus.ProjectDoesNotExist, Enumerable.Empty<(string, string)>());
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Error getting corresponding variable groups for {repository} repository, {project} azure project.",
                repositoryName,
                project
                );
            return (AdapterStatus.Unknown, Enumerable.Empty<(string, string)>());
        }
    }

    private async Task<IEnumerable<(string, string)>> GetVariableGroupNames(
        string project,
        ReleaseDefinition definition,
        CancellationToken cancellationToken
        )
    {
        var taskAgentClient = await _connection.GetClientAsync<TaskAgentHttpClient>(cancellationToken: cancellationToken);
        var variableGroupNames = new List<(string, string)>();

        foreach (var env in definition.Environments.Where(env => !ExcludableEnvironments.Any(env.Name.Contains)))
        {
            foreach (var id in env.VariableGroups)
            {
                var vg = await taskAgentClient.GetVariableGroupAsync(project, id, cancellationToken: cancellationToken);
                variableGroupNames.Add((vg.Name, vg.Type));
            }
        }

        return variableGroupNames;
    }

    private async Task<ReleaseDefinition?> GetReleaseDefinitionAsync(
        string organization,
        string pat,
        string project,
        string repositoryName,
        string configFile,
        CancellationToken cancellationToken
        )
    {
        Setup(organization, pat);
        var releaseClient = await _connection.GetClientAsync<ReleaseHttpClient>(cancellationToken);
        var expand = ReleaseDefinitionExpands.Artifacts;
        var releaseDefinitions = await releaseClient.GetReleaseDefinitionsAsync(
            project,
            expand: expand,
            cancellationToken: cancellationToken
            );

        var foundDefinitions = releaseDefinitions.Where(
            definition => definition.Artifacts.Any(artifact =>
            {
                var artifactType = artifact.DefinitionReference.GetValueOrDefault("definition")?.Name;
                return artifactType?.Equals(repositoryName) ?? false;
            })
            );

        ReleaseDefinition? definition = null!;

        foreach (var def in foundDefinitions)
        {
            var subResult = await releaseClient.GetReleaseDefinitionAsync(project, def?.Id ?? 0, cancellationToken: cancellationToken);

            var workFlowTasks = subResult?.Environments.FirstOrDefault()?.DeployPhases.FirstOrDefault()?.WorkflowTasks.ToList() ?? 
                Enumerable.Empty<WorkflowTask>();
            foreach(var task in workFlowTasks)
            {
                task.Inputs.TryGetValue("configuration", out var configValue);
                task.Inputs.TryGetValue("command", out var command);

                if ((configValue?.Contains(configFile) ?? false) && command == "apply")
                {
                    definition = subResult;
                }
            }
        }

        return definition;
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
