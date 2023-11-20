using VGManager.Entities;
using VGManager.Repositories.Interfaces.Boilerplate;

namespace VGManager.Repositories.Interfaces;

public interface IVGAddColdRepository : ISqlRepository<AdditionEntity>
{
    Task AddEntityAsync(AdditionEntity entity, CancellationToken cancellationToken = default);
    Task<IEnumerable<AdditionEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<AdditionEntity>> GetAsync(
        string organization,
        string project,
        string user,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default
        );
    Task<IEnumerable<AdditionEntity>> GetAsync(
        string organization,
        string project,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default
        );
}
