using VGManager.Entities.SecretEntities;
using VGManager.Repositories.Boilerplate;
using VGManager.Repositories.DbContexts;
using VGManager.Repositories.Interfaces.SecretRepositories;

namespace VGManager.Repositories.SecretRepositories;

public class KeyVaultCopyColdRepository : SqlRepository<KeyVaultCopyEntity>, IKeyVaultCopyColdRepository
{
    public KeyVaultCopyColdRepository(OperationsDbContext dbContext) : base(dbContext)
    {
    }

    public async Task AddEntityAsync(KeyVaultCopyEntity entity, CancellationToken cancellationToken = default)
    {
        await AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<KeyVaultCopyEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await GetAllAsync(new KeyVaultCopySpecification(), cancellationToken);
        return result?.ToList() ?? Enumerable.Empty<KeyVaultCopyEntity>();
    }

    public class KeyVaultCopySpecification : SpecificationBase<KeyVaultCopyEntity>
    {
        public KeyVaultCopySpecification() : base(entity => !string.IsNullOrEmpty(entity.Id))
        {
        }
    }
}
