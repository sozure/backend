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

    public async Task Add(DeletionEntity entity, CancellationToken cancellationToken = default)
    {
        await AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }
}
