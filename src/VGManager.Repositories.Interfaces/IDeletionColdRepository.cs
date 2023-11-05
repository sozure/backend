using VGManager.Entities;

namespace VGManager.Repositories.Interfaces;

public interface IDeletionColdRepository
{
    Task Add(DeletionEntity entity, CancellationToken cancellationToken = default);
}
