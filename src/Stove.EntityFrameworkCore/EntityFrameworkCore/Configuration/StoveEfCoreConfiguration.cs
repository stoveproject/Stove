using System;

using Autofac.Extras.IocManager;

using Microsoft.EntityFrameworkCore;

namespace Stove.EntityFrameworkCore.Configuration
{
	public class StoveEfCoreConfiguration : IStoveEfCoreConfiguration, ISingletonDependency
	{
		public void AddDbContext<TDbContext>(IIocBuilder builder,
			Func<IStoveDbContextConfigurer<TDbContext>, IStoveDbContextConfigurer<TDbContext>> action) where TDbContext : DbContext
		{
			builder.RegisterServices(r => { r.Register(ctx => action); });
		}
	}
}
