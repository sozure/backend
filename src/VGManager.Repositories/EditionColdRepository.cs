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

    public async Task AddEntityAsync(EditionEntity entity, CancellationToken cancellationToken = default)
    {
        await AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task<EditionEntity[]> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await GetAllAsync(new EditionSpecification(), cancellationToken);
    }

    public async Task<EditionEntity[]> GetByDateAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default
        )
    {
        return await GetAllAsync(new EditionSpecification(from, to), cancellationToken);
    }

    public class EditionSpecification : SpecificationBase<EditionEntity>
    {
        public EditionSpecification() : base(editionEntity => !string.IsNullOrEmpty(editionEntity.Id))
        {
        }

        public EditionSpecification(DateTime from, DateTime to) : base(
            editionEntity => editionEntity.Date >= from && editionEntity.Date <= to
            )
        {
        }
    }
}
