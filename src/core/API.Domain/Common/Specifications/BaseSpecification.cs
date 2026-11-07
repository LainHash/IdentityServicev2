using API.Domain.Extensions;
using System.Linq.Expressions;

namespace API.Domain.Common.Specifications
{
    public abstract class BaseSpecification<TEntity> : ISpecification<TEntity> where TEntity : class
    {
        public Expression<Func<TEntity, bool>>? Criteria { get; protected set; }

        public List<Expression<Func<TEntity, object>>> Includes { get; }
            = new();

        public List<string> IncludeStrings { get; }
            = new();

        public List<Func<IQueryable<TEntity>, IQueryable<TEntity>>> IncludeAggregators { get; }
            = new();

        public Expression<Func<TEntity, object>>? OrderBy { get; private set; }

        public Expression<Func<TEntity, object>>? OrderByDescending { get; private set; }

        public List<Expression<Func<TEntity, object>>> ThenBy { get; }
            = new();

        public List<Expression<Func<TEntity, object>>> ThenByDescending { get; }
            = new();

        public int Skip { get; private set; }

        public int Take { get; private set; }

        public bool IsPagingEnabled { get; private set; }

        public bool IgnoreQueryFilters { get; private set; }

        public bool IsSoftDeleteEnabled { get; private set; }

        protected void AddCriteria(Expression<Func<TEntity, bool>> criteria)
        {
            Criteria = Criteria is null ? criteria : Criteria.And(criteria);
        }

        protected void AddIgnoreQueryFilters()
        {
            IgnoreQueryFilters = true;
        }

        protected void EnableSoftDeleteFilter()
        {
            IsSoftDeleteEnabled = true;
        }

        protected void AddInclude(Expression<Func<TEntity, object>> include)
        {
            Includes.Add(include);
        }

        protected void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }

        protected void AddIncludeAggregator(Func<IQueryable<TEntity>, IQueryable<TEntity>> aggregator)
        {
            IncludeAggregators.Add(aggregator);
        }

        protected void ApplyOrderBy(Expression<Func<TEntity, object>> orderBy)
        {
            OrderBy = orderBy;
        }

        protected void ApplyOrderByDescending(Expression<Func<TEntity, object>> orderByDescending)
        {
            OrderByDescending = orderByDescending;
        }

        protected void ApplyThenBy(Expression<Func<TEntity, object>> thenBy)
        {
            ThenBy.Add(thenBy);
        }

        protected void ApplyThenByDescending(Expression<Func<TEntity, object>> thenByDescending)
        {
            ThenByDescending.Add(thenByDescending);
        }

        protected void ApplyPaging(int page, int pageSize)
        {
            Skip = (page - 1) * pageSize;
            Take = pageSize;
            IsPagingEnabled = true;
        }
    }
}
