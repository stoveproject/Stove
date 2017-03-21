using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Threading.Tasks;

using Autofac.Extras.IocManager;

using JetBrains.Annotations;

using Stove.Domain.Uow;
using Stove.EntityFramework.Utils;
using Stove.Extensions;

namespace Stove.EntityFramework.Uow
{
    public class EfUnitOfWorkDbContextContainer
    {
        public DbContext DbContext { get; set; }

        public ObjectMaterializedEventHandler ObjectMaterializedDelegate { get; set; }
    }

    /// <summary>
    ///     Implements Unit of work for Entity Framework.
    /// </summary>
    public class EfUnitOfWork : UnitOfWorkBase, ITransientDependency
    {
        private readonly IDbContextResolver _dbContextResolver;
        private readonly IDbContextTypeMatcher _dbContextTypeMatcher;
        private readonly IEfTransactionStrategy _transactionStrategy;

        /// <summary>
        ///     Creates a new <see cref="EfUnitOfWork" />.
        /// </summary>
        public EfUnitOfWork(
            IConnectionStringResolver connectionStringResolver,
            IDbContextResolver dbContextResolver,
            IEfUnitOfWorkFilterExecuter filterExecuter,
            IUnitOfWorkDefaultOptions defaultOptions,
            IDbContextTypeMatcher dbContextTypeMatcher,
            IEfTransactionStrategy transactionStrategy)
            : base(
                connectionStringResolver,
                defaultOptions,
                filterExecuter)
        {
            _dbContextResolver = dbContextResolver;
            _dbContextTypeMatcher = dbContextTypeMatcher;
            _transactionStrategy = transactionStrategy;

            ActiveDbContexts = new Dictionary<string, EfUnitOfWorkDbContextContainer>();
        }

        [NotNull]
        protected IDictionary<string, EfUnitOfWorkDbContextContainer> ActiveDbContexts { get; }

        protected override void BeginUow()
        {
            if (Options.IsTransactional == true)
            {
                _transactionStrategy.InitOptions(Options);
            }
        }

        public override void SaveChanges()
        {
            GetAllActiveDbContexts().ForEach(SaveChangesInDbContext);
        }

        public override async Task SaveChangesAsync()
        {
            foreach (DbContext dbContext in GetAllActiveDbContexts())
            {
                await SaveChangesInDbContextAsync(dbContext);
            }
        }

        [NotNull]
        public IReadOnlyList<DbContext> GetAllActiveDbContexts()
        {
            return ActiveDbContexts.Values.Select(x=>x.DbContext).ToImmutableList();
        }

        protected override void CompleteUow()
        {
            SaveChanges();

            if (Options.IsTransactional == true)
            {
                _transactionStrategy.Commit();
            }

            DisposeUow();
        }

        protected override async Task CompleteUowAsync()
        {
            await SaveChangesAsync();

            if (Options.IsTransactional == true)
            {
                _transactionStrategy.Commit();
            }

            DisposeUow();
        }

        [NotNull]
        public virtual TDbContext GetOrCreateDbContext<TDbContext>()
            where TDbContext : DbContext
        {
            Type concreteDbContextType = _dbContextTypeMatcher.GetConcreteType(typeof(TDbContext));

            var connectionStringResolveArgs = new ConnectionStringResolveArgs();
            connectionStringResolveArgs["DbContextType"] = typeof(TDbContext);
            connectionStringResolveArgs["DbContextConcreteType"] = concreteDbContextType;
            string connectionString = ResolveConnectionString(connectionStringResolveArgs);

            string dbContextKey = concreteDbContextType.FullName + "#" + connectionString;

            EfUnitOfWorkDbContextContainer dbContextContainer;
            if (!ActiveDbContexts.TryGetValue(dbContextKey, out dbContextContainer))
            {
                dbContextContainer = new EfUnitOfWorkDbContextContainer();
                if (Options.IsTransactional == true)
                {
                    dbContextContainer.DbContext = _transactionStrategy.CreateDbContext<TDbContext>(connectionString, _dbContextResolver);
                }
                else
                {
                    dbContextContainer.DbContext = _dbContextResolver.Resolve<TDbContext>(connectionString);
                }

                if (Options.Timeout.HasValue && !dbContextContainer.DbContext.Database.CommandTimeout.HasValue)
                {
                    dbContextContainer.DbContext.Database.CommandTimeout = Options.Timeout.Value.TotalSeconds.To<int>();
                }

                ((IObjectContextAdapter)dbContextContainer.DbContext).ObjectContext.ObjectMaterialized += (sender, args) =>
                {
                    ObjectContext_ObjectMaterialized(dbContextContainer.DbContext, args);
                };

                FilterExecuter.As<IEfUnitOfWorkFilterExecuter>().ApplyCurrentFilters(this, dbContextContainer.DbContext);

                ActiveDbContexts[dbContextKey] = dbContextContainer;
            }

            return (TDbContext)dbContextContainer.DbContext;
        }

        protected override void DisposeUow()
        {
            foreach (var dbContextContainer in ActiveDbContexts.Values)
            {
                if (dbContextContainer.ObjectMaterializedDelegate != null)
                {
                    ((IObjectContextAdapter)dbContextContainer.DbContext).ObjectContext.ObjectMaterialized -= dbContextContainer.ObjectMaterializedDelegate;
                    dbContextContainer.ObjectMaterializedDelegate = null;
                }
            }

            if (Options.IsTransactional == true)
            {
                _transactionStrategy.Dispose();
            }
            else
            {
                foreach (EfUnitOfWorkDbContextContainer dbContextContainer in ActiveDbContexts.Values)
                {
                    Release(dbContextContainer);
                }
            }

            ActiveDbContexts.Clear();
        }

        protected virtual void SaveChangesInDbContext([NotNull] DbContext dbContext)
        {
            dbContext.SaveChanges();
        }

        protected virtual async Task SaveChangesInDbContextAsync([NotNull] DbContext dbContext)
        {
            await dbContext.SaveChangesAsync();
        }

        protected virtual void Release([NotNull] EfUnitOfWorkDbContextContainer dbContextContainer)
        {
            dbContextContainer.DbContext.Dispose();
        }

        private void ObjectContext_ObjectMaterialized([NotNull] DbContext dbContext, ObjectMaterializedEventArgs e)
        {
            Type entityType = ObjectContext.GetObjectType(e.Entity.GetType());

            dbContext.Configuration.AutoDetectChangesEnabled = false;
            EntityState previousState = dbContext.Entry(e.Entity).State;

            DateTimePropertyInfoHelper.NormalizeDatePropertyKinds(e.Entity, entityType);

            dbContext.Entry(e.Entity).State = previousState;
            dbContext.Configuration.AutoDetectChangesEnabled = true;
        }
    }
}
