using System.Collections.Generic;
using System.Reflection;

using Autofac.Extras.IocManager;

using JetBrains.Annotations;

using Stove.Domain.Entities;
using Stove.Domain.Uow;
using Stove.EntityFramework;
using Stove.EntityFramework.Uow;
using Stove.Orm;

namespace Stove
{
	public static class StoveEntityFrameworkRegistrationExtensions
	{
		/// <summary>
		///     Uses the stove entity framework.
		/// </summary>
		/// <param name="builder">The builder.</param>
		/// <returns></returns>
		[NotNull]
		public static IIocBuilder UseStoveEntityFramework([NotNull] this IIocBuilder builder)
		{
			return builder.RegisterServices(r =>
			              {
				              var ormRegistrars = new List<ISecondaryOrmRegistrar>();
				              r.OnRegistering += (sender, args) =>
				              {
					              if (typeof(StoveDbContext).IsAssignableFrom(args.ImplementationType))
					              {
						              EfRepositoryRegistrar.RegisterRepositories(args.ImplementationType, builder);
						              ormRegistrars.Add(new EfBasedSecondaryOrmRegistrar(builder, args.ImplementationType, DbContextHelper.GetEntityTypeInfos, EntityHelper.GetPrimaryKeyType));
						              args.ContainerBuilder.Properties[StoveConsts.OrmRegistrarContextKey] = ormRegistrars;
					              }
				              };

				              r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
				              r.Register<IUnitOfWorkFilterExecuter, IEfUnitOfWorkFilterExecuter, EfDynamicFiltersUnitOfWorkFilterExecuter>();
				              r.RegisterGeneric(typeof(IDbContextProvider<>), typeof(UnitOfWorkDbContextProvider<>));
				              r.Register<IUnitOfWorkDefaultOptions, UnitOfWorkDefaultOptions>(Lifetime.Singleton);
			              })
			              .UseStoveEntityFrameworkCommon();
		}

		/// <summary>
		///     Uses the repository registrar in assembly.
		/// </summary>
		/// <param name="builder">The builder.</param>
		/// <param name="assembly">The assembly.</param>
		/// <returns></returns>
		[NotNull]
		public static IIocBuilder UseRepositoryRegistrarInAssembly([NotNull] this IIocBuilder builder, [NotNull] Assembly assembly)
		{
			Check.NotNull(assembly, nameof(assembly));

			builder.RegisterServices(r =>
			{
				r.OnRegistering += (sender, args) =>
				{
					if (typeof(StoveDbContext).IsAssignableFrom(args.ImplementationType))
					{
						EfRepositoryRegistrar.RegisterRepositories(args.ImplementationType, builder);
					}
				};
			});

			return builder;
		}

		/// <summary>
		///     Uses the stove transaction scope ef transaction strategy.
		/// </summary>
		/// <param name="builder">The builder.</param>
		/// <returns></returns>
		[NotNull]
		public static IIocBuilder UseStoveTransactionScopeEfTransactionStrategy([NotNull] this IIocBuilder builder)
		{
			return builder.RegisterServices(r => r.Register<IEfTransactionStrategy, TransactionScopeEfTransactionStrategy>());
		}

		/// <summary>
		///     Uses the stove database context ef transaction strategy.
		/// </summary>
		/// <param name="builder">The builder.</param>
		/// <returns></returns>
		[NotNull]
		public static IIocBuilder UseStoveDbContextEfTransactionStrategy([NotNull] this IIocBuilder builder)
		{
			return builder.RegisterServices(r => r.Register<IEfTransactionStrategy, DbContextEfTransactionStrategy>());
		}
	}
}
