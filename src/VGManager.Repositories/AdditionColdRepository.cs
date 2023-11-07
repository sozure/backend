using Microsoft.EntityFrameworkCore;
using VGManager.Entities;
using VGManager.Repositories.Boilerplate;
using VGManager.Repositories.Interfaces;

namespace VGManager.Repositories;

public class AdditionColdRepository : SqlRepository<AdditionEntity>, IAdditionColdRepository
{
    public AdditionColdRepository(DbContext dbContext) : base(dbContext)
    {
    }

    public async Task Add(AdditionEntity entity, CancellationToken cancellationToken = default)
    {
        await AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }
}
