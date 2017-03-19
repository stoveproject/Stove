using System;
using System.Linq.Expressions;

using DapperExtensions;

using JetBrains.Annotations;

using Stove.Domain.Entities;

namespace Stove.Dapper.Expressions
{
    /// <summary>
    ///     http://stackoverflow.com/questions/15154783/pulling-apart-expressionfunct-object
    ///     http://stackoverflow.com/questions/16083895/grouping-lambda-expressions-by-operators-and-using-them-with-dapperextensions-p
    ///     http://blogs.msdn.com/b/mattwar/archive/2007/07/31/linq-building-an-iqueryable-provider-part-ii.aspx
    ///     http://msdn.microsoft.com/en-us/library/bb546136(v=vs.110).aspx
    ///     http://stackoverflow.com/questions/14437239/change-a-linq-expression-predicate-from-one-type-to-another/14439071#14439071
    /// </summary>
    internal static class DapperExpressionExtensions
    {
        public static IPredicate ToPredicateGroup<TEntity, TPrimaryKey>([NotNull] this Expression<Func<TEntity, bool>> expression) where TEntity : class, IEntity<TPrimaryKey>
        {
            Check.NotNull(expression, nameof(expression));

            var dev = new DapperExpressionVisitor<TEntity, TPrimaryKey>();
            IPredicate pg = dev.Process(expression);

            return pg;
        }
    }
}
