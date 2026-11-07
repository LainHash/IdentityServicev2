using API.Domain.Common.Entities;
using API.Domain.Common.Specifications;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure.Common.Specifications
{
    internal static class SpecificationEvaluator
    {
        public static IQueryable<TEntity> GetQuery<TEntity>(
            IQueryable<TEntity> query,
            ISpecification<TEntity> spec,
            bool applyPaging = true) where TEntity : class
        {
            if (spec.IgnoreQueryFilters)
            {
                query = query.IgnoreQueryFilters();
            }

            if (spec.IsSoftDeleteEnabled && typeof(SoftDeletableEntity).IsAssignableFrom(typeof(TEntity)))
            {
                query = query.Where(x => EF.Property<bool>(x, "IsDeleted") == false);
            }

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            if (spec.Includes != null)
            {
                query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (spec.IncludeStrings != null)
            {
                query = spec.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));
            }

            if (spec.IncludeAggregators != null)
            {
                query = spec.IncludeAggregators.Aggregate(query, (current, aggregator) => aggregator(current));
            }

            if (spec.OrderBy != null)
            {
                var orderedQuery = query.OrderBy(spec.OrderBy);

                orderedQuery = spec.ThenBy.Aggregate(orderedQuery, (current, thenBy) => current.ThenBy(thenBy));
                orderedQuery = spec.ThenByDescending.Aggregate(orderedQuery, (current, thenByDesc) => current.ThenByDescending(thenByDesc));

                query = orderedQuery;
            }
            else if (spec.OrderByDescending != null)
            {
                var orderedQuery = query.OrderByDescending(spec.OrderByDescending);

                orderedQuery = spec.ThenBy.Aggregate(orderedQuery, (current, thenBy) => current.ThenBy(thenBy));
                orderedQuery = spec.ThenByDescending.Aggregate(orderedQuery, (current, thenByDesc) => current.ThenByDescending(thenByDesc));

                query = orderedQuery;
            }

            if (applyPaging && spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            return query;
        }
    }
}
