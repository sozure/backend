using VGManager.AzureAdapter.Entities;

namespace VGManager.AzureAdapter.Interfaces;

public interface IReleasePipelineAdapter
{
    Task<(AdapterStatus, IEnumerable<string>)> GetEnvironmentsAsync(
        string organization,
        string pat,
        string project,
        string repositoryName,
        CancellationToken cancellationToken = default
        );

    Task<(AdapterStatus, IEnumerable<string>)> GetVariableGroupsAsync(
        string organization,
        string pat,
        string project,
        string repositoryName,
        CancellationToken cancellationToken = default
        );
}
