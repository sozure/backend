using Microsoft.TeamFoundation.Build.WebApi;

namespace VGManager.AzureAdapter.Interfaces;

public interface IBuildPipelineAdapter
{
    Task<IEnumerable<BuildDefinitionReference>> GetBuildPipelinesAsync(
        string organization,
        string pat,
        string project,
        CancellationToken cancellationToken = default
        );
    Task RunBuildPipelineAsync(
        string organization,
        string pat,
        string project,
        int definitionId,
        string sourceBranch,
        CancellationToken cancellationToken = default
        );
}
