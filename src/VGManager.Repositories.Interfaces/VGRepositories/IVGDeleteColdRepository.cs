using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using VGManager.Entities;
using VGManager.Repositories.Interfaces.Boilerplate;

namespace VGManager.Repositories.Interfaces.VGRepositories;

public interface IVGDeleteColdRepository : ISqlRepository<DeletionEntity>
{
    Task AddEntityAsync(DeletionEntity entity, CancellationToken cancellationToken = default);
    Task<IEnumerable<DeletionEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<DeletionEntity>> GetAsync(
        string organization,
        string project,
        string user,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default
        );
    Task<IEnumerable<DeletionEntity>> GetAsync(
        string organization,
        string project,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default
        );
}
