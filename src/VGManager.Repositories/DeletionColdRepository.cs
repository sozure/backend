using Humanizer;
using VGManager.Entities;
using VGManager.Repositories.Boilerplate;
using VGManager.Repositories.DbContexts;
using VGManager.Repositories.Interfaces;

namespace VGManager.Repositories;

public class DeletionColdRepository : SqlRepository<DeletionEntity>, IDeletionColdRepository
{
    public DeletionColdRepository(OperationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task AddEntityAsync(DeletionEntity entity, CancellationToken cancellationToken = default)
    {
        await AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<DeletionEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await GetAllAsync(new DeletionSpecification(), cancellationToken);
        return result?.ToList() ?? Enumerable.Empty<DeletionEntity>();
    }

    public async Task<IEnumerable<DeletionEntity>> GetByDateAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default
        )
    {
        var result = await GetAllAsync(new DeletionSpecification(from, to), cancellationToken);
        return result?.ToList() ?? Enumerable.Empty<DeletionEntity>();
    }

    public class DeletionSpecification : SpecificationBase<DeletionEntity>
    {
        public DeletionSpecification() : base(deletionEntity => !string.IsNullOrEmpty(deletionEntity.Id))
        {
        }

        public DeletionSpecification(DateTime from, DateTime to) : base(
            deletionEntity => deletionEntity.Date >= from && deletionEntity.Date <= to
            )
        {
        }
    }
}
