using System;

using Autofac.Extras.IocManager;

using Microsoft.EntityFrameworkCore;

namespace Stove.EntityFrameworkCore.Configuration
{
	public interface IStoveEfCoreConfiguration
	{
		void AddDbContext<TDbContext>(IIocBuilder builder,
			Func<IStoveDbContextConfigurer<TDbContext>, IStoveDbContextConfigurer<TDbContext>> action) where TDbContext : DbContext;
	}
}
