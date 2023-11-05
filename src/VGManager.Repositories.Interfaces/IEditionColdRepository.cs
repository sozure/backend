using VGManager.Entities;

namespace VGManager.Repositories.Interfaces;

public interface IEditionColdRepository
{
    Task Add(EditionEntity entity, CancellationToken cancellationToken = default);
}
