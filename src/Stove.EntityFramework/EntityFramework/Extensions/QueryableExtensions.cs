using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

using Autofac.Extras.IocManager;

using Stove.Domain.Entities;
using Stove.Domain.Repositories;
using Stove.EntityFramework.EntityFramework.Interceptors;
using Stove.Extensions;

namespace Stove.EntityFramework.EntityFramework.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="IQueryable" /> and <see cref="IQueryable{T}" />.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        ///     Specifies the related objects to include in the query results.
        /// </summary>
        /// <param name="source">The source <see cref="IQueryable" /> on which to call Include.</param>
        /// <param name="condition">A boolean value to determine to include <paramref name="path" /> or not.</param>
        /// <param name="path">The dot-separated list of related objects to return in the query results.</param>
        public static IQueryable IncludeIf(this IQueryable source, bool condition, string path)
        {
            return condition
                ? source.Include(path)
                : source;
        }

        /// <summary>
        ///     Specifies the related objects to include in the query results.
        /// </summary>
        /// <param name="source">The source <see cref="IQueryable{T}" /> on which to call Include.</param>
        /// <param name="condition">A boolean value to determine to include <paramref name="path" /> or not.</param>
        /// <param name="path">The dot-separated list of related objects to return in the query results.</param>
        public static IQueryable<T> IncludeIf<T>(this IQueryable<T> source, bool condition, string path)
        {
            return condition
                ? source.Include(path)
                : source;
        }

        /// <summary>
        ///     Specifies the related objects to include in the query results.
        /// </summary>
        /// <param name="source">The source <see cref="IQueryable{T}" /> on which to call Include.</param>
        /// <param name="condition">A boolean value to determine to include <paramref name="path" /> or not.</param>
        /// <param name="path">The type of navigation property being included.</param>
        public static IQueryable<T> IncludeIf<T, TProperty>(this IQueryable<T> source, bool condition, Expression<Func<T, TProperty>> path)
        {
            return condition
                ? source.Include(path)
                : source;
        }

        /// <summary>
        ///     Nolockings the specified queryable.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult">The type of the result.</typeparam>
        /// <param name="repository">The repository.</param>
        /// <param name="queryable">The queryable.</param>
        /// <returns></returns>
        public static TResult Nolocking<T, TResult>(this IRepository<T, int> repository, Func<IQueryable<T>, TResult> queryable) where T : class, IEntity<int>
        {
            Check.NotNull(queryable, nameof(queryable));

            TResult result;
            using (IScopeResolver scopeResolver = repository.As<IStoveRepositoryBaseWithResolver>().ScopeResolver.BeginScope())
            {
                using (scopeResolver.Resolve<WithNoLockInterceptor>().UseNolocking())
                {
                    result = queryable(repository.GetAll());
                }
            }

            return result;
        }
    }
}
