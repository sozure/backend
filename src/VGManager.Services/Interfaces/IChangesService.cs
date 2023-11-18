using VGManager.Services.Models.Changes;

namespace VGManager.Services.Interfaces;

public interface IChangesService
{
    Task<IEnumerable<OperationModel>> GetAsync(
        RequestModel model,
        CancellationToken cancellationToken = default
        );
}
