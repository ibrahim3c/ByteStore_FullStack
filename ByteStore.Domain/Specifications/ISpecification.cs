using System.Linq.Expressions;

namespace ByteStore.Domain.Specifications
{
    public interface ISpecification<TEntity> where TEntity : class
    {
        Expression<Func<TEntity, bool>>? Criteria { get; }
        List<Expression<Func<TEntity, object>>> IncludeExpressions { get; }
        Expression<Func<TEntity, object>>? OrderByExpression { get; }
        Expression<Func<TEntity, object>>? OrderByDescendingExpression { get; }
        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }
    }
}
