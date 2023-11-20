using VGManager.Entities.VGEntities;
using VGManager.Repositories.Boilerplate;
using VGManager.Repositories.DbContexts;
using VGManager.Repositories.Interfaces.VGRepositories;

namespace VGManager.Repositories.VGRepositories;

public class VGDeleteColdRepository : SqlRepository<VGDeleteEntity>, IVGDeleteColdRepository
{
    public VGDeleteColdRepository(OperationsDbContext dbContext) : base(dbContext)
    {
    }

    public async Task AddEntityAsync(VGDeleteEntity entity, CancellationToken cancellationToken = default)
    {
        await AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<VGDeleteEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await GetAllAsync(new DeletionSpecification(), cancellationToken);
        return result?.ToList() ?? Enumerable.Empty<VGDeleteEntity>();
    }

    public async Task<IEnumerable<VGDeleteEntity>> GetAsync(
        string organization,
        string project,
        string user,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default
        )
    {
        var result = await GetAllAsync(new DeletionSpecification(organization, project, user, from, to), cancellationToken);
        return result?.ToList() ?? Enumerable.Empty<VGDeleteEntity>();
    }

    public async Task<IEnumerable<VGDeleteEntity>> GetAsync(
        string organization,
        string project,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default
        )
    {
        var result = await GetAllAsync(new DeletionSpecification(organization, project, from, to), cancellationToken);
        return result?.ToList() ?? Enumerable.Empty<VGDeleteEntity>();
    }

    public class DeletionSpecification : SpecificationBase<VGDeleteEntity>
    {
        public DeletionSpecification() : base(deletionEntity => !string.IsNullOrEmpty(deletionEntity.Id))
        {
        }

        public DeletionSpecification(string organization, string project, string user, DateTime from, DateTime to) : base(
            deletionEntity => deletionEntity.Date >= from &&
            deletionEntity.Date <= to &&
            deletionEntity.Organization == organization &&
            deletionEntity.Project == project &&
            deletionEntity.User.Contains(user)
            )
        {
        }

        public DeletionSpecification(string organization, string project, DateTime from, DateTime to) : base(
            deletionEntity => deletionEntity.Date >= from &&
            deletionEntity.Date <= to &&
            deletionEntity.Organization == organization &&
            deletionEntity.Project == project
            )
        {
        }
    }
}
