using System;
using System.Data.Common;
using System.Reflection;

using Autofac.Extras.IocManager;

using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using Stove.EntityFramework.Common;
using Stove.EntityFrameworkCore.Configuration;

namespace Stove.EntityFrameworkCore
{
	public class DefaultDbContextResolver : IDbContextResolver, ITransientDependency
	{
		private static readonly MethodInfo CreateOptionsMethod = typeof(DefaultDbContextResolver).GetMethod("CreateOptions", BindingFlags.NonPublic | BindingFlags.Instance);
		private readonly IDbContextTypeMatcher _dbContextTypeMatcher;

		private readonly IScopeResolver _scopeResolver;

		public DefaultDbContextResolver(
			IDbContextTypeMatcher dbContextTypeMatcher, IScopeResolver scopeResolver)
		{
			_dbContextTypeMatcher = dbContextTypeMatcher;
			_scopeResolver = scopeResolver;
		}

		public TDbContext Resolve<TDbContext>(string connectionString, DbConnection existingConnection)
			where TDbContext : DbContext
		{
			Type dbContextType = typeof(TDbContext);

			if (!dbContextType.GetTypeInfo().IsAbstract)
			{
				return _scopeResolver.Resolve<TDbContext>(new
				{
					options = CreateOptions<TDbContext>(connectionString, existingConnection)
				});
			}

			Type concreteType = _dbContextTypeMatcher.GetConcreteType(dbContextType);

			return (TDbContext)_scopeResolver.Resolve(concreteType, new
			{
				options = CreateOptionsForType(concreteType, connectionString, existingConnection)
			});
		}

		private object CreateOptionsForType(Type dbContextType, string connectionString, DbConnection existingConnection)
		{
			return CreateOptionsMethod.MakeGenericMethod(dbContextType).Invoke(this, new object[] { connectionString, existingConnection });
		}

		protected virtual DbContextOptions<TDbContext> CreateOptions<TDbContext>(
			[NotNull] string connectionString,
			[CanBeNull] DbConnection existingConnection) where TDbContext : DbContext
		{
			if (_scopeResolver.IsRegistered<IStoveDbContextConfigurer<TDbContext>>())
			{
				var configuration = new StoveDbContextConfiguration<TDbContext>(connectionString, existingConnection);

				configuration.DbContextOptions.ReplaceService<IEntityMaterializerSource, StoveEntityMaterializerSource>();

				_scopeResolver.Resolve<IStoveDbContextConfigurer<TDbContext>>().Configure(configuration);

				return configuration.DbContextOptions.Options;
			}

			if (_scopeResolver.IsRegistered<DbContextOptions<TDbContext>>())
			{
				return _scopeResolver.Resolve<DbContextOptions<TDbContext>>();
			}

			throw new StoveException($"Could not resolve DbContextOptions for {typeof(TDbContext).AssemblyQualifiedName}.");
		}
	}
}
