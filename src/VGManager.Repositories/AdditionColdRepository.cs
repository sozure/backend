using VGManager.Entities;
using VGManager.Repositories.Boilerplate;
using VGManager.Repositories.DbContexts;
using VGManager.Repositories.Interfaces;

namespace VGManager.Repositories;

public class AdditionColdRepository : SqlRepository<AdditionEntity>, IAdditionColdRepository
{
    public AdditionColdRepository(OperationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task Add(AdditionEntity entity, CancellationToken cancellationToken = default)
    {
        await AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }
}
