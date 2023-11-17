using VGManager.Entities;
using VGManager.Repositories.Interfaces.Boilerplate;

namespace VGManager.Repositories.Interfaces;

public interface IAdditionColdRepository : ISqlRepository<AdditionEntity>
{
    Task AddEntityAsync(AdditionEntity entity, CancellationToken cancellationToken = default);
    Task<IEnumerable<AdditionEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<AdditionEntity>> GetByDateAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default
        );
}
