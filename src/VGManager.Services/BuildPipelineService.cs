using Microsoft.TeamFoundation.Build.WebApi;
using VGManager.AzureAdapter.Interfaces;
using VGManager.Models.StatusEnums;
using VGManager.Services.Interfaces;

namespace VGManager.Services;

public class BuildPipelineService : IBuildPipelineService
{
    private readonly IBuildPipelineAdapter _buildPipelineAdapter;
    private readonly IGitRepositoryAdapter _gitRepositoryAdapter;

    public BuildPipelineService(IBuildPipelineAdapter buildPipelineAdapter, IGitRepositoryAdapter gitRepositoryAdapter)
    {
        _buildPipelineAdapter = buildPipelineAdapter;
        _gitRepositoryAdapter = gitRepositoryAdapter;
    }

    public async Task<Guid> GetRepositoryIdByBuildDefinitionAsync(
        string organization,
        string pat,
        string project,
        int id,
        CancellationToken cancellationToken = default
        )
    {
        var pipeline = await _buildPipelineAdapter.GetBuildPipelineAsync(organization, pat, project, id, cancellationToken);
        var repositories = await _gitRepositoryAdapter.GetAllAsync(organization, project, pat, cancellationToken);
        var repo = repositories.FirstOrDefault(r => r.Name == pipeline.Name);
        return repo?.Id ?? Guid.Empty;
    }

    public async Task<IEnumerable<Dictionary<string, string>>> GetBuildPipelinesAsync(
        string organization,
        string pat,
        string project,
        CancellationToken cancellationToken = default
        )
    {
        var result = new List<Dictionary<string, string>>();
        var pipelines = await _buildPipelineAdapter.GetBuildPipelinesAsync(organization, pat, project, cancellationToken);

        foreach (var pipeline in pipelines)
        {
            result.Add(
                new() { ["name"] = pipeline.Name, ["id"] = pipeline.Id.ToString() });
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
