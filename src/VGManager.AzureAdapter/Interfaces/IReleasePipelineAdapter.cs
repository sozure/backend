using VGManager.Models;

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

    Task<(AdapterStatus, IEnumerable<(string, string)>)> GetVariableGroupsAsync(
        string organization,
        string pat,
        string project,
        string repositoryName,
        CancellationToken cancellationToken = default
        );
}
