using Microsoft.Extensions.Logging;
using Microsoft.TeamFoundation.Build.WebApi;
using VGManager.AzureAdapter.Interfaces;

namespace VGManager.AzureAdapter;

public class BuildPipelineAdapter: IBuildPipelineAdapter
{

    private readonly IHttpClientProvider _clientProvider;
    private readonly ILogger _logger;

    public BuildPipelineAdapter(IHttpClientProvider clientProvider, ILogger<BuildPipelineAdapter> logger)
    {
        _clientProvider = clientProvider;
        _logger = logger;
    }

    public async Task<IEnumerable<BuildDefinitionReference>> GetBuildPipelinesAsync(
        string organization,
        string pat,
        string project,
        CancellationToken cancellationToken = default
        )
    {
        _logger.LogInformation("Request build pipelines from Azure DevOps.");
        _clientProvider.Setup(organization, pat);
        using var client = await _clientProvider.GetClientAsync<BuildHttpClient>(cancellationToken);
        return await client.GetDefinitionsAsync(project, cancellationToken: cancellationToken);
    }

    /// <summary>
    /// Run build pipeline
    /// </summary>
    /// <param name="organization"></param>
    /// <param name="pat"></param>
    /// <param name="project"></param>
    /// <param name="definitionId"></param>
    /// <param name="sourceBranch">for example: "refs/tags/2.1.1" or "refs/heads/develop"</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task RunBuildPipelineAsync(
        string organization, 
        string pat,
        string project,
        int definitionId,
        string sourceBranch,
        CancellationToken cancellationToken = default
        )
    {
        _logger.LogInformation("Request build pipelines from Azure DevOps.");
        _clientProvider.Setup(organization, pat);
        using var client = await _clientProvider.GetClientAsync<BuildHttpClient>(cancellationToken);
        var pipeline = await client.GetDefinitionAsync(project, definitionId, cancellationToken: cancellationToken);
        var build = new Build
        {
            Definition = pipeline,
            Project = pipeline.Project,
            SourceBranch = sourceBranch
        };
        var finishedBuild = await client
            .QueueBuildAsync(build, true, definitionId: pipeline.Id, cancellationToken: cancellationToken);
    }
}