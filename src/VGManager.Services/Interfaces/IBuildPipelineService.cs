using Microsoft.TeamFoundation.Build.WebApi;
using VGManager.Models.StatusEnums;

namespace VGManager.Services.Interfaces;

public interface IBuildPipelineService
{
    Task<Guid> GetRepositoryIdByBuildDefinitionAsync(
        string organization,
        string pat,
        string project,
        int id,
        CancellationToken cancellationToken = default
        );

    Task<IEnumerable<Dictionary<string, string>>> GetBuildPipelinesAsync(
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
