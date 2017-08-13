using System.Collections.Generic;
using System.Reflection;

using Autofac.Extras.IocManager;

using Stove.Domain.Entities;
using Stove.Domain.Uow;
using Stove.EntityFramework.Common;
using Stove.EntityFrameworkCore;
using Stove.EntityFrameworkCore.Uow;
using Stove.Orm;
using Stove.Reflection.Extensions;

namespace Stove
{
	public static class StoveEntityFrameworkCoreRegistrationExtensions
	{
		public static IIocBuilder UseStoveEntityFrameworkCore(this IIocBuilder builder)
		{
			return builder
				.RegisterServices(r =>
				{
					var ormRegistrars = new List<ISecondaryOrmRegistrar>();
					r.OnRegistering += (sender, args) =>
					{
						if (typeof(StoveDbContext).IsAssignableFrom(args.ImplementationType))
						{
							EfCoreRepositoryRegistrar.RegisterRepositories(args.ImplementationType, builder);
							ormRegistrars.Add(new EfCoreBasedSecondaryOrmRegistrar(builder, args.ImplementationType, EfCoreDbContextEntityFinder.GetEntityTypeInfos, EntityHelper.GetPrimaryKeyType));
							args.ContainerBuilder.Properties[StoveConsts.OrmRegistrarContextKey] = ormRegistrars;
						}
					};

					r.RegisterAssemblyByConvention(typeof(StoveEntityFrameworkCoreRegistrationExtensions).GetAssembly());
					r.RegisterGeneric(typeof(IDbContextProvider<>), typeof(UnitOfWorkDbContextProvider<>));
					r.Register<IUnitOfWorkDefaultOptions, UnitOfWorkDefaultOptions>(Lifetime.Singleton);
				})
				.UseStoveEntityFrameworkCommon();
		}
	}
}
