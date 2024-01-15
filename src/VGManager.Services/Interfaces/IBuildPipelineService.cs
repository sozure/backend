using VGManager.Models.StatusEnums;

namespace VGManager.Services.Interfaces;

public interface IBuildPipelineService
{
    Task<IEnumerable<(string, string)>> GetBuildPipelinesAsync(
        string organization,
        string pat,
        string project,
        CancellationToken cancellationToken = default
        );

    Task<AdapterStatus> RunBuildPipelineAsync(
        string organization,
        string pat,
        string project,
        int definitionId,
        string sourceBranch,
        CancellationToken cancellationToken = default
        );
}
