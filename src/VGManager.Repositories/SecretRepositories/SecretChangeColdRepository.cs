using VGManager.Entities.SecretEntities;
using VGManager.Repositories.Boilerplate;
using VGManager.Repositories.DbContexts;
using VGManager.Repositories.Interfaces.SecretRepositories;

namespace VGManager.Repositories.SecretRepositories;

public class SecretChangeColdRepository : SqlRepository<SecretChangeEntity>, ISecretChangeColdRepository
{
    public SecretChangeColdRepository(OperationsDbContext dbContext) : base(dbContext)
    {
    }

    public async Task AddEntityAsync(SecretChangeEntity entity, CancellationToken cancellationToken = default)
    {
        await AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<SecretChangeEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await GetAllAsync(new SecretChangeSpecification(), cancellationToken);
        return result?.ToList() ?? Enumerable.Empty<SecretChangeEntity>();
    }

    public class SecretChangeSpecification : SpecificationBase<SecretChangeEntity>
    {
        public SecretChangeSpecification() : base(entity => !string.IsNullOrEmpty(entity.Id))
        {
        }
    }
}
