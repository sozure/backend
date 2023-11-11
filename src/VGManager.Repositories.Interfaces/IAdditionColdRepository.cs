using VGManager.Entities;
using VGManager.Repositories.Interfaces.Boilerplate;

namespace VGManager.Repositories.Interfaces;

public interface IAdditionColdRepository: ISqlRepository<AdditionEntity>
{
    Task Add(AdditionEntity entity, CancellationToken cancellationToken = default);
}
