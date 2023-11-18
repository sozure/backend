using VGManager.Services.Models.Changes;

namespace VGManager.Services.Interfaces;

public interface IChangesService
{
    Task<IEnumerable<OperationModel>> GetAsync(
        int limit,
        IEnumerable<ChangeType> changeTypes,
        CancellationToken cancellationToken = default
        );
}
