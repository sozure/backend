using VGManager.Entities.SecretEntities;
using VGManager.Repositories.Interfaces.Boilerplate;

namespace VGManager.Repositories.Interfaces.SecretRepositories;

public interface ISecretChangeColdRepository: ISqlRepository<SecretChangeEntity>
{
    Task AddEntityAsync(SecretChangeEntity entity, CancellationToken cancellationToken = default);
    Task<IEnumerable<SecretChangeEntity>> GetAllAsync(CancellationToken cancellationToken = default);
}
