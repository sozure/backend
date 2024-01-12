using VGManager.Models.StatusEnums;

namespace VGManager.Services.Interfaces;

public interface IBuildPipelineService
{
    Task<AdapterStatus> RunBuildPipelineAsync(
        string organization,
        string pat,
        string project,
        int definitionId,
        string sourceBranch,
        CancellationToken cancellationToken = default
        );
}
