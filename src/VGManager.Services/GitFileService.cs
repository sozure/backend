using VGManager.Adapter.Models.Kafka;
using VGManager.Adapter.Models.Requests;
using VGManager.Adapter.Models.StatusEnums;
using VGManager.Services.Interfaces;

namespace VGManager.Services;

public class GitFileService(
    IGitAdapterCommunicatorService gitAdapterCommunicatorService
        ) : IGitFileService
{
    public async Task<(AdapterStatus, IEnumerable<string>)> GetFilePathAsync(
        string organization,
        string pat,
        string repositoryId,
        string fileName,
        string branch,
        CancellationToken cancellationToken = default
        )
    {
        var commandType = CommandTypes.GetFilePathRequest;

        return await GetInformationAsync(
            commandType,
            organization,
            pat,
            repositoryId,
            branch,
            fileName,
            cancellationToken
            );
    }

    public async Task<(AdapterStatus, IEnumerable<string>)> GetConfigFilesAsync(
        string organization,
        string pat,
        string repositoryId,
        string? extension,
        string branch,
        CancellationToken cancellationToken = default
        )
    {
        var commandType = CommandTypes.GetConfigFilesRequest;

        return await GetInformationAsync(
            commandType,
            organization,
            pat,
            repositoryId,
            branch,
            extension,
            cancellationToken
            );
    }

    private async Task<(AdapterStatus, IEnumerable<string>)> GetInformationAsync(
        string commandType,
        string organization,
        string pat,
        string repositoryId,
        string branch,
        string? additionalInformation,
        CancellationToken cancellationToken
        )
    {
        var request = new GitFileBaseRequest<string>()
        {
            Organization = organization,
            PAT = pat,
            Branch = branch,
            RepositoryId = repositoryId,
            AdditionalInformation = additionalInformation,
        };

        return await gitAdapterCommunicatorService.GetInformationAsync(commandType, request, cancellationToken);
    }
}
