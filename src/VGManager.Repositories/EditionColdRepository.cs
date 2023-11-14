using VGManager.Entities;
using VGManager.Repositories.Boilerplate;
using VGManager.Repositories.DbContexts;
using VGManager.Repositories.Interfaces;

namespace VGManager.Repositories;

public class EditionColdRepository : SqlRepository<EditionEntity>, IEditionColdRepository
{
    public EditionColdRepository(OperationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task Add(EditionEntity entity, CancellationToken cancellationToken = default)
    {
        await AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }
}
