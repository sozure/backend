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

    public async Task AddEntityAsync(AdditionEntity entity, CancellationToken cancellationToken = default)
    {
        await AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task<AdditionEntity[]> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await GetAllAsync(new AdditionSpecification(), cancellationToken);
    }

    public async Task<AdditionEntity[]> GetByDateAsync(
        DateTime from, 
        DateTime to, 
        CancellationToken cancellationToken = default
        )
    {
        return await GetAllAsync(new AdditionSpecification(from, to), cancellationToken);
    }

    public class AdditionSpecification : SpecificationBase<AdditionEntity>
    {
        public AdditionSpecification() : base(additionEntity => !string.IsNullOrEmpty(additionEntity.Id))
        {
        }

        public AdditionSpecification(DateTime from, DateTime to) : base(
            additionEntity => additionEntity.Date >= from && additionEntity.Date <= to
            )
        {
        }
    }
}
