using VGManager.Entities;
using VGManager.Repositories.Boilerplate;
using VGManager.Repositories.DbContexts;
using VGManager.Repositories.Interfaces.VGRepositories;

namespace VGManager.Repositories.VGRepositories;

public class VGAddColdRepository : SqlRepository<VGAddEntity>, IVGAddColdRepository
{
    public VGAddColdRepository(OperationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task AddEntityAsync(VGAddEntity entity, CancellationToken cancellationToken = default)
    {
        await AddAsync(entity, cancellationToken);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<VGAddEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var result = await GetAllAsync(new AdditionSpecification(), cancellationToken);
        return result?.ToList() ?? Enumerable.Empty<VGAddEntity>();
    }

    public async Task<IEnumerable<VGAddEntity>> GetAsync(
        string organization,
        string project,
        string user,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default
        )
    {
        var result = await GetAllAsync(new AdditionSpecification(organization, project, user, from, to), cancellationToken);
        return result?.ToList() ?? Enumerable.Empty<VGAddEntity>();
    }

    public async Task<IEnumerable<VGAddEntity>> GetAsync(
        string organization,
        string project,
        DateTime from,
        DateTime to,
        CancellationToken cancellationToken = default
        )
    {
        var result = await GetAllAsync(new AdditionSpecification(organization, project, from, to), cancellationToken);
        return result?.ToList() ?? Enumerable.Empty<VGAddEntity>();
    }

    public class AdditionSpecification : SpecificationBase<VGAddEntity>
    {
        public AdditionSpecification() : base(additionEntity => !string.IsNullOrEmpty(additionEntity.Id))
        {
        }

        public AdditionSpecification(string organization, string project, string user, DateTime from, DateTime to) : base(
            additionEntity => additionEntity.Date >= from &&
            additionEntity.Date <= to &&
            additionEntity.Organization == organization &&
            additionEntity.Project == project &&
            additionEntity.User.Contains(user)
            )
        {
        }

        public AdditionSpecification(string organization, string project, DateTime from, DateTime to) : base(
            additionEntity => additionEntity.Date >= from &&
            additionEntity.Date <= to &&
            additionEntity.Organization == organization &&
            additionEntity.Project == project
            )
        {
        }
    }
}
