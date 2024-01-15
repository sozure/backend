using VGManager.AzureAdapter.Interfaces;
using VGManager.Models.StatusEnums;
using VGManager.Services.Interfaces;

namespace VGManager.Services;

public class BuildPipelineService : IBuildPipelineService
{
    private readonly IBuildPipelineAdapter _buildPipelineAdapter;

    public BuildPipelineService(IBuildPipelineAdapter buildPipelineAdapter)
    {
        _buildPipelineAdapter = buildPipelineAdapter;
    }

    public async Task<IEnumerable<(string, string)>> GetBuildPipelinesAsync(
        string organization,
        string pat,
        string project,
        CancellationToken cancellationToken = default
        )
    {
        var result = new List<(string, string)>();
        var pipelines = await _buildPipelineAdapter.GetBuildPipelinesAsync(organization, pat, project, cancellationToken);

        foreach (var pipeline in pipelines)
        {
            result.Add((pipeline.Name, pipeline.Id.ToString()));
        }

        return result;
    }

    public async Task<AdapterStatus> RunBuildPipelineAsync(
        string organization,
        string pat,
        string project,
        int definitionId,
        string sourceBranch,
        CancellationToken cancellationToken = default
        )
    {
        return await _buildPipelineAdapter.RunBuildPipelineAsync(
            organization,
            pat,
            project,
            definitionId,
            sourceBranch,
            cancellationToken
            );
    }
}
