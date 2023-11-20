using Microsoft.EntityFrameworkCore;
using VGManager.Entities;
using VGManager.Entities.Configurations;

namespace VGManager.Repositories.DbContexts;

public class OperationDbContext : DbContext
{
    #region Contructor(s)

    /// <summary>
    /// Initialize a new instance of <see cref="OperationDbContext"/> for unit testing purposes.
    /// Moq needs this to construct a Mocked version
    /// </summary>
    public OperationDbContext()
    {
    }

    /// <summary>
    /// Initialize a new instance of <see cref="OperationDbContext"/>
    /// </summary>
    /// <param name="options">DbContext options</param>
    public OperationDbContext(DbContextOptions<OperationDbContext> options)
        : base(options)
    {
    }

    #endregion

    #region DbSets

    public DbSet<VGAddEntity> Additions { get; set; } = null!;
    public DbSet<VGDeleteEntity> Deletions { get; set; } = null!;
    public DbSet<VGUpdateEntity> Editions { get; set; } = null!;

    #endregion

    #region Overriden Methods

    /// <summary>
    /// Overriden Method to apply entity configurations
    /// </summary>
    /// <param name="modelBuilder">The model builder to configuration</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new AdditionEntityConfig());
        modelBuilder.ApplyConfiguration(new DeletionEntityConfig());
        modelBuilder.ApplyConfiguration(new EditionEntityConfig());
    }

    #endregion
}
