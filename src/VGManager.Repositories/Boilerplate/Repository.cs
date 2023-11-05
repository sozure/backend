using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VGManager.Repositories.Interfaces.Boilerplate;

namespace VGManager.Repositories.Boilerplate;
public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
{
    //
    // Summary:
    //     The DbContext
    protected DbContext dbContext;

    //
    // Summary:
    //     The Entity
    protected DbSet<TEntity> dbSet;

    //
    // Summary:
    //     Initialize a new instance of SMP.Repository.Core.Repository`1
    //
    // Parameters:
    //   dbContext:
    //     The current DbContext
    protected Repository(DbContext dbContext)
    {
        this.dbContext = dbContext;
        dbSet = this.dbContext.Set<TEntity>();
    }

    public virtual Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
    {
        return dbSet.AddRangeAsync(entities, cancellationToken);
    }

    public virtual ValueTask<TEntity?> FindAsync(object[] keys, CancellationToken cancellationToken = default)
    {
        return dbSet.FindAsync(keys, cancellationToken);
    }

    public virtual void Remove(TEntity entity)
    {
        dbSet.Remove(entity);
    }

    public virtual void RemoveRange(IEnumerable<TEntity> entities)
    {
        dbSet.RemoveRange(entities);
    }

    public virtual void UpdateRange(IEnumerable<TEntity> entities)
    {
        dbSet.UpdateRange(entities);
    }
}
