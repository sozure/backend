using VGManager.Entities;
using VGManager.Repositories.Interfaces.Boilerplate;

namespace VGManager.Repositories.Interfaces;

public interface IEditionColdRepository : ISqlRepository<EditionEntity>
{
    Task Add(EditionEntity entity, CancellationToken cancellationToken = default);
}
