using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace API.Domain.Extensions
{
    public static class StringComparisonExtensions
    {
        private static readonly MethodInfo StringTrimMethod =
            typeof(string).GetMethod(nameof(string.Trim), Type.EmptyTypes)!;

        private static readonly MethodInfo StringToLowerMethod =
            typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes)!;

        private static readonly MethodInfo EfLikeMethod =
            typeof(DbFunctionsExtensions).GetMethod(
                nameof(DbFunctionsExtensions.Like),
                [typeof(DbFunctions), typeof(string), typeof(string)])!;

        private static readonly MemberExpression EfFunctionsProperty =
            Expression.Property(null, typeof(EF), nameof(EF.Functions));

        public static bool LikeEquals(
            this string? source,
            string? target,
            bool ignoreCase = true)
        {
            if (source is null || target is null)
                return source is null && target is null;

            var left = source.Trim();
            var right = target.Trim();

            if (ignoreCase)
            {
                left = left.ToLowerInvariant();
                right = right.ToLowerInvariant();
            }

            return EF.Functions.Like(left, right);
        }

        public static bool LikeContains(
            this string? source,
            string? keyword,
            bool ignoreCase = true)
        {
            if (source is null || keyword is null)
                return false;

            var left = source.Trim();
            var right = keyword.Trim();

            if (ignoreCase)
            {
                left = left.ToLowerInvariant();
                right = right.ToLowerInvariant();
            }

            return EF.Functions.Like(left, $"%{right}%");
        }

        public static Expression<Func<T, bool>> StringEquals<T>(
            Expression<Func<T, string?>> propertySelector,
            string? value,
            bool ignoreCase = true)
        {
            var trimmed = (value ?? string.Empty).Trim();
            var pattern = ignoreCase ? trimmed.ToLowerInvariant() : trimmed;
            return BuildLikeExpression(propertySelector, pattern, ignoreCase);
        }

        public static Expression<Func<T, bool>> StringContains<T>(
            Expression<Func<T, string?>> propertySelector,
            string? keyword,
            bool ignoreCase = true)
        {
            var trimmed = (keyword ?? string.Empty).Trim();
            var normalized = ignoreCase ? trimmed.ToLowerInvariant() : trimmed;
            return BuildLikeExpression(propertySelector, $"%{normalized}%", ignoreCase);
        }

        public static Expression<Func<T, bool>> StringLike<T>(
            Expression<Func<T, string?>> propertySelector,
            string? pattern,
            bool ignoreCase = true)
        {
            var trimmed = (pattern ?? string.Empty).Trim();
            var normalized = ignoreCase ? trimmed.ToLowerInvariant() : trimmed;
            return BuildLikeExpression(propertySelector, normalized, ignoreCase);
        }

        public static IQueryable<T> WhereStringEquals<T>(
            this IQueryable<T> query,
            Expression<Func<T, string?>> propertySelector,
            string? value,
            bool ignoreCase = true)
        {
            return query.Where(StringEquals(propertySelector, value, ignoreCase));
        }

        public static IQueryable<T> WhereStringContains<T>(
            this IQueryable<T> query,
            Expression<Func<T, string?>> propertySelector,
            string? keyword,
            bool ignoreCase = true)
        {
            return query.Where(StringContains(propertySelector, keyword, ignoreCase));
        }

        public static Task<T?> FirstOrDefaultStringEqualsAsync<T>(
            this IQueryable<T> query,
            Expression<Func<T, string?>> propertySelector,
            string? value,
            bool ignoreCase = true,
            CancellationToken cancellationToken = default)
        {
            return query.FirstOrDefaultAsync(
                StringEquals(propertySelector, value, ignoreCase),
                cancellationToken);
        }

        public static Task<bool> AnyStringEqualsAsync<T>(
            this IQueryable<T> query,
            Expression<Func<T, string?>> propertySelector,
            string? value,
            bool ignoreCase = true,
            CancellationToken cancellationToken = default)
        {
            return query.AnyAsync(
                StringEquals(propertySelector, value, ignoreCase),
                cancellationToken);
        }

        private static Expression<Func<T, bool>> BuildLikeExpression<T>(
            Expression<Func<T, string?>> propertySelector,
            string pattern,
            bool ignoreCase)
        {
            var parameter = propertySelector.Parameters[0];
            var propertyBody = propertySelector.Body;

            var notNullCheck = Expression.NotEqual(
                propertyBody,
                Expression.Constant(null, typeof(string)));

            Expression processedProperty = Expression.Call(propertyBody, StringTrimMethod);

            if (ignoreCase)
            {
                processedProperty = Expression.Call(processedProperty, StringToLowerMethod);
            }

            var likeCall = Expression.Call(
                null,
                EfLikeMethod,
                EfFunctionsProperty,
                processedProperty,
                Expression.Constant(pattern, typeof(string)));

            var combinedBody = Expression.AndAlso(notNullCheck, likeCall);

            return Expression.Lambda<Func<T, bool>>(combinedBody, parameter);
        }
    }
}

