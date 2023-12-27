using System.Linq.Expressions;

namespace VGManager.Repositories.Interfaces.Boilerplate;

public interface ISpecification<TEntity>
{
    Expression<Func<TEntity, bool>> Criteria { get; }

    ICollection<Expression<Func<TEntity, object>>> Includes { get; }

    ICollection<string> IncludeStrings { get; }
}
