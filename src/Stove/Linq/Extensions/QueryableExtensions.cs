using System;
using System.Linq;
using System.Linq.Expressions;

using JetBrains.Annotations;

namespace Stove.Linq.Extensions
{
    /// <summary>
    ///     Some useful extension methods for <see cref="IQueryable{T}" />.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        ///     Used for paging. Can be used as an alternative to Skip(...).Take(...) chaining.
        /// </summary>
        [NotNull]
        public static IQueryable<T> PageBy<T>([NotNull] this IQueryable<T> query, int skipCount, int maxResultCount)
        {
            Check.NotNull(query, nameof(query));

            return query.Skip(skipCount).Take(maxResultCount);
        }

        /// <summary>
        ///     Filters a <see cref="IQueryable{T}" /> by given predicate if given condition is true.
        /// </summary>
        /// <param name="query">Queryable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the query</param>
        /// <returns>Filtered or not filtered query based on <paramref name="condition" /></returns>
        [NotNull]
        public static IQueryable<T> WhereIf<T>([NotNull] this IQueryable<T> query, bool condition, [NotNull] Expression<Func<T, bool>> predicate)
        {
            Check.NotNull(query, nameof(query));
            Check.NotNull(predicate, nameof(predicate));

            return condition
                ? query.Where(predicate)
                : query;
        }

        /// <summary>
        ///     Filters a <see cref="IQueryable{T}" /> by given predicate if given condition is true.
        /// </summary>
        /// <param name="query">Queryable to apply filtering</param>
        /// <param name="condition">A boolean value</param>
        /// <param name="predicate">Predicate to filter the query</param>
        /// <returns>Filtered or not filtered query based on <paramref name="condition" /></returns>
        [NotNull]
        public static IQueryable<T> WhereIf<T>([NotNull] this IQueryable<T> query, bool condition, [NotNull] Expression<Func<T, int, bool>> predicate)
        {
            Check.NotNull(query, nameof(query));
            Check.NotNull(predicate, nameof(predicate));

            return condition
                ? query.Where(predicate)
                : query;
        }
    }
}
