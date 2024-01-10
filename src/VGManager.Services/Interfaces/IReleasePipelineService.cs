using VGManager.Models.StatusEnums;

namespace VGManager.Services.Interfaces;

public interface IReleasePipelineService
{
    Task<(AdapterStatus, IEnumerable<string>)> GetProjectsWhichHaveCorrespondingReleasePipelineAsync(
        string organization,
        string pat,
        IEnumerable<string> projects,
        string repositoryName,
        string configFile,
        CancellationToken cancellationToken = default
        );

    Task<(AdapterStatus, IEnumerable<string>)> GetEnvironmentsAsync(
        string organization,
        string pat,
        string project,
        string repositoryName,
        string configFile,
        CancellationToken cancellationToken = default
        );

    Task<(AdapterStatus, IEnumerable<(string, string)>)> GetVariableGroupsAsync(
        string organization,
        string pat,
        string project,
        string repositoryName,
        string configFile,
        CancellationToken cancellationToken = default
        );
}
