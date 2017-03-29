using System;
using System.Collections.Generic;
using System.Linq;

using Autofac.Extras.IocManager;

using Stove.Collections.Extensions;
using Stove.Domain.Repositories;
using Stove.Domain.Uow;

namespace Stove.EntityFramework
{
    public class DbContextTypeMatcher : DbContextTypeMatcher<StoveDbContext>
    {
        public DbContextTypeMatcher(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
            : base(currentUnitOfWorkProvider)
        {
        }
    }

    public abstract class DbContextTypeMatcher<TBaseDbContext> : IDbContextTypeMatcher, ISingletonDependency
    {
        private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;
        private readonly Dictionary<Type, List<Type>> _dbContextTypes;

        protected DbContextTypeMatcher(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
        {
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
            _dbContextTypes = new Dictionary<Type, List<Type>>();
        }

        /// <summary>
        ///     Populates the specified database context types.
        /// </summary>
        /// <param name="dbContextTypes">The database context types.</param>
        public void Populate(Type[] dbContextTypes)
        {
            foreach (Type dbContextType in dbContextTypes)
            {
                var types = new List<Type>();

                AddWithBaseTypes(dbContextType, types);

                foreach (Type type in types)
                {
                    Add(type, dbContextType);
                }
            }
        }

        /// <summary>
        ///     Gets the type of the concrete.
        /// </summary>
        /// <param name="sourceDbContextType">Type of the source database context.</param>
        /// <returns></returns>
        /// <exception cref="StoveException">
        ///     Could not find a concrete implementation of given DbContext type: " +
        ///     sourceDbContextType.AssemblyQualifiedName
        /// </exception>
        public virtual Type GetConcreteType(Type sourceDbContextType)
        {
            if (!sourceDbContextType.IsAbstract)
            {
                return sourceDbContextType;
            }

            //Get possible concrete types for given DbContext type
            List<Type> allTargetTypes = _dbContextTypes.GetOrDefault(sourceDbContextType);

            if (allTargetTypes.IsNullOrEmpty())
            {
                throw new StoveException("Could not find a concrete implementation of given DbContext type: " + sourceDbContextType.AssemblyQualifiedName);
            }

            if (allTargetTypes.Count == 1)
            {
                //Only one type does exists, return it
                return allTargetTypes[0];
            }

            CheckCurrentUow();

            return GetDefaultDbContextType(allTargetTypes, sourceDbContextType);
        }

        private void CheckCurrentUow()
        {
            if (_currentUnitOfWorkProvider.Current == null)
            {
                throw new StoveException("GetConcreteType method should be called in a UOW.");
            }
        }

        private static Type GetDefaultDbContextType(List<Type> dbContextTypes, Type sourceDbContextType)
        {
            List<Type> filteredTypes = dbContextTypes
                .Where(type => !type.IsDefined(typeof(AutoRepositoryTypesAttribute), true))
                .ToList();

            if (filteredTypes.Count == 1)
            {
                return filteredTypes[0];
            }

            filteredTypes = filteredTypes
                .Where(type => !type.IsDefined(typeof(DefaultDbContextAttribute), true))
                .ToList();

            if (filteredTypes.Count == 1)
            {
                return filteredTypes[0];
            }

            throw new StoveException(string.Format(
                "Found more than one concrete type for given DbContext Type ({0}). Found types: {2}.",
                sourceDbContextType,
                dbContextTypes.Select(c => c.AssemblyQualifiedName).JoinAsString(", ")
            ));
        }

        private static void AddWithBaseTypes(Type dbContextType, List<Type> types)
        {
            types.Add(dbContextType);
            if (dbContextType != typeof(TBaseDbContext))
            {
                AddWithBaseTypes(dbContextType.BaseType, types);
            }
        }

        private void Add(Type sourceDbContextType, Type targetDbContextType)
        {
            if (!_dbContextTypes.ContainsKey(sourceDbContextType))
            {
                _dbContextTypes[sourceDbContextType] = new List<Type>();
            }

            _dbContextTypes[sourceDbContextType].Add(targetDbContextType);
        }
    }
}
