using System;
using System.Linq.Expressions;
using System.Reflection;

using Autofac.Extras.IocManager;

using Stove.Domain.Entities;
using Stove.Domain.Uow;
using Stove.Reflection.Extensions;
using Stove.Utils;

namespace Stove.RavenDB.Filters.Query
{
    public class SoftDeleteRavenQueryFilter : IRavenQueryFilter, ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public SoftDeleteRavenQueryFilter(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public bool IsDeleted => false;

        public string FilterName { get; } = StoveDataFilters.SoftDelete;

        public bool IsEnabled => _unitOfWorkManager.Current.IsFilterEnabled(FilterName);

        public Expression<Func<TEntity, bool>> ExecuteFilter<TEntity, TPrimaryKey>() where TEntity : class, IEntity<TPrimaryKey>
        {
            Expression<Func<TEntity, bool>> predicate = null;
            if (IsFilterable<TEntity, TPrimaryKey>())
            {
                PropertyInfo propType = typeof(TEntity).GetProperty(nameof(ISoftDelete.IsDeleted));
                predicate = ExpressionUtils.MakePredicate<TEntity>(nameof(ISoftDelete.IsDeleted), IsDeleted, propType.PropertyType);
            }
            return predicate;
        }

        private bool IsFilterable<TEntity, TPrimaryKey>() where TEntity : class, IEntity<TPrimaryKey>
        {
            return typeof(TEntity).IsInheritsOrImplements(typeof(ISoftDelete)) && IsEnabled;
        }
    }
}
