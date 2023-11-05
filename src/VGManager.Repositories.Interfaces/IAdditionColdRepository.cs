using VGManager.Entities;

namespace VGManager.Repositories.Interfaces;

public interface IAdditionColdRepository
{
    Task Add(AdditionEntity entity, CancellationToken cancellationToken = default);
}
