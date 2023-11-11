using VGManager.Entities;
using VGManager.Repositories.Interfaces.Boilerplate;

namespace VGManager.Repositories.Interfaces;

public interface IDeletionColdRepository : ISqlRepository<DeletionEntity>
{
    Task Add(DeletionEntity entity, CancellationToken cancellationToken = default);
}
