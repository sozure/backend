using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using VGManager.Entities.VGEntities;
using VGManager.Repositories.Interfaces.Boilerplate;

namespace VGManager.Repositories.Interfaces.VGRepositories;

public interface IVGDeleteColdRepository : ISqlRepository<VGDeleteEntity>
{
    Task AddEntityAsync(VGDeleteEntity entity, CancellationToken cancellationToken = default);
    Task<IEnumerable<VGDeleteEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<VGDeleteEntity>> GetAsync(
        string organization,
        string project,
        string user,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default
        );
    Task<IEnumerable<VGDeleteEntity>> GetAsync(
        string organization,
        string project,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default
        );
}
