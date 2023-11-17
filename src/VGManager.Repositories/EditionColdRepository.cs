using VGManager.Entities;
using VGManager.Repositories.Boilerplate;
using VGManager.Repositories.DbContexts;
using VGManager.Repositories.Interfaces;
using static VGManager.Repositories.DeletionColdRepository;

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

    public async Task<IEnumerable<EditionEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await GetAllAsync(new EditionSpecification(), cancellationToken);
        return result?.ToList() ?? Enumerable.Empty<EditionEntity>();
    }

    public async Task<IEnumerable<EditionEntity>> GetByDateAsync(
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default
        )
    {
        var result = await GetAllAsync(new EditionSpecification(from, to), cancellationToken);
        return result?.ToList() ?? Enumerable.Empty<EditionEntity>();
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
