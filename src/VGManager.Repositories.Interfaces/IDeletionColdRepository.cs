using VGManager.Entities;
using VGManager.Repositories.Interfaces.Boilerplate;

namespace VGManager.Repositories.Interfaces;

public interface IDeletionColdRepository : ISqlRepository<DeletionEntity>
{
    Task AddEntityAsync(DeletionEntity entity, CancellationToken cancellationToken = default);
    Task<IEnumerable<DeletionEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<DeletionEntity>> GetByDateAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default
        );
}
