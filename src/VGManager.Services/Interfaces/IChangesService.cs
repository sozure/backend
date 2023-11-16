using VGManager.Entities;
using VGManager.Services.Models.Changes;

namespace VGManager.Services.Interfaces;

public interface IChangesService
{
    Task<IEnumerable<OperationModel>> GetByDateAsync(
        DateTime from, 
        DateTime to, 
        ChangeType[] changeTypes, 
        CancellationToken cancellationToken = default
        );
    
    Task<IEnumerable<OperationModel>> GetByMaxLimitAsync(
        int limit, 
        ChangeType[] changeTypes, 
        CancellationToken cancellationToken = default
        );
}
