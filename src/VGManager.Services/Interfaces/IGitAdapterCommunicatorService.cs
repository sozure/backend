using VGManager.Adapter.Models.StatusEnums;

namespace VGManager.Services.Interfaces;

public interface IGitAdapterCommunicatorService
{
    Task<(AdapterStatus, IEnumerable<string>)> GetInformationAsync<T>(
        string commandType,
        T request,
        CancellationToken cancellationToken
        );
}
