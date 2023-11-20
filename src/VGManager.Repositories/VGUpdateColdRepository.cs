using VGManager.Entities;
using VGManager.Repositories.Boilerplate;
using VGManager.Repositories.DbContexts;
using VGManager.Repositories.Interfaces;

namespace VGManager.Repositories;

public class VGUpdateColdRepository : SqlRepository<EditionEntity>, IVGUpdateColdRepository
{
    public VGUpdateColdRepository(OperationDbContext dbContext) : base(dbContext)
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

    public async Task<IEnumerable<EditionEntity>> GetAsync(
        string organization,
        string project,
        string user,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default
        )
    {
        var result = await GetAllAsync(new EditionSpecification(organization, project, user, from, to), cancellationToken);
        return result?.ToList() ?? Enumerable.Empty<EditionEntity>();
    }

    public async Task<IEnumerable<EditionEntity>> GetAsync(
        string organization,
        string project,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default
        )
    {
        var result = await GetAllAsync(new EditionSpecification(organization, project, from, to), cancellationToken);
        return result?.ToList() ?? Enumerable.Empty<EditionEntity>();
    }

    public class EditionSpecification : SpecificationBase<EditionEntity>
    {
        public EditionSpecification() : base(editionEntity => !string.IsNullOrEmpty(editionEntity.Id))
        {
        }

        public EditionSpecification(string organization, string project, string user, DateTime from, DateTime to) : base(
            editionEntity => editionEntity.Date >= from && 
            editionEntity.Date <= to &&
            editionEntity.Organization == organization &&
            editionEntity.Project == project &&
            editionEntity.User.Contains(user)
            )
        {
        }

        public EditionSpecification(string organization, string project, DateTime from, DateTime to) : base(
            editionEntity => editionEntity.Date >= from &&
            editionEntity.Date <= to &&
            editionEntity.Organization == organization &&
            editionEntity.Project == project
            )
        {
        }
    }
}
