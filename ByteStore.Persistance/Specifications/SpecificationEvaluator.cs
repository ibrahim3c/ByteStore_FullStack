using ByteStore.Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace ByteStore.Persistance.Specifications
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> GetQuery<TEntity>(
            IQueryable<TEntity> inputQuerable,
            ISpecification<TEntity> specification) where TEntity : class
        {
            IQueryable<TEntity> querable = inputQuerable;

            if (specification.Criteria is not null)
            {
                querable = querable.Where(specification.Criteria);
            }

            querable = specification.IncludeExpressions.Aggregate(
                querable, (current, includeExpression) => current.Include(includeExpression));

            if (specification.OrderByExpression is not null)
            {
                querable = querable.OrderBy(specification.OrderByExpression);
            }
            else if (specification.OrderByDescendingExpression is not null)
            {
                querable = querable.OrderByDescending(specification.OrderByDescendingExpression);
            }

            if (specification.IsPagingEnabled)
            {
                querable = querable.Skip(specification.Skip).Take(specification.Take);
            }


            return querable;
        }
    }
}
