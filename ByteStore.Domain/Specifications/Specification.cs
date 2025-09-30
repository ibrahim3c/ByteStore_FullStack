using ByteStore.Domain.Specifications;
using System.Linq.Expressions;

namespace BytStore.Application.Specifications
{
    public class Specification<TEntity> :ISpecification<TEntity> where TEntity : class 
    {
        protected Specification() { }
        protected Specification(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = criteria;
        }
        public Expression<Func<TEntity, bool>> Criteria { get; }
        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = new();
        public Expression<Func<TEntity, object>>? OrderByExpression { get; private set; }
        public Expression<Func<TEntity, object>>? OrderByDescendingExpression { get; private set; }
        public int Take { get; private set; }
        public int Skip { get; private set; }
        public bool IsPagingEnabled { get; private set; }
        protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)=> IncludeExpressions.Add(includeExpression);
        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)=> OrderByExpression = orderByExpression;
        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescendingExpression) => OrderByDescendingExpression = orderByDescendingExpression;
        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }

    }
}
