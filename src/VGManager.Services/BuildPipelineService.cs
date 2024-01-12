using VGManager.AzureAdapter.Interfaces;
using VGManager.Models.StatusEnums;
using VGManager.Services.Interfaces;

namespace VGManager.Services;

public class BuildPipelineService: IBuildPipelineService
{
    private readonly IBuildPipelineAdapter _buildPipelineAdapter;

    public BuildPipelineService(IBuildPipelineAdapter buildPipelineAdapter)
    {
        _buildPipelineAdapter = buildPipelineAdapter;
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
