using VGManager.Entities.SecretEntities;
using VGManager.Repositories.Interfaces.Boilerplate;

namespace VGManager.Repositories.Interfaces.SecretRepositories;

public interface IKeyVaultCopyColdRepository : ISqlRepository<KeyVaultCopyEntity>
{
    Task AddEntityAsync(KeyVaultCopyEntity entity, CancellationToken cancellationToken = default);
    Task<IEnumerable<KeyVaultCopyEntity>> GetAllAsync(CancellationToken cancellationToken = default);
}
