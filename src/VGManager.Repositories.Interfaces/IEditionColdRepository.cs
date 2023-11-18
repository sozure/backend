using VGManager.Entities;
using VGManager.Repositories.Interfaces.Boilerplate;

namespace VGManager.Repositories.Interfaces;

public interface IEditionColdRepository : ISqlRepository<EditionEntity>
{
    Task AddEntityAsync(EditionEntity entity, CancellationToken cancellationToken = default);
    Task<IEnumerable<EditionEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<EditionEntity>> GetAsync(
        string organization,
        string project,
        string user,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default
        );
    Task<IEnumerable<EditionEntity>> GetAsync(
        string organization,
        string project,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default
        );
}
