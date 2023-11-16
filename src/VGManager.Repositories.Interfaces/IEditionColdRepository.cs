using VGManager.Entities;
using VGManager.Repositories.Interfaces.Boilerplate;

namespace VGManager.Repositories.Interfaces;

public interface IEditionColdRepository : ISqlRepository<EditionEntity>
{
    Task AddEntityAsync(EditionEntity entity, CancellationToken cancellationToken = default);
    Task<EditionEntity[]> GetAllAsync(CancellationToken cancellationToken = default);
    Task<EditionEntity[]> GetByDateAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default
        );
}
