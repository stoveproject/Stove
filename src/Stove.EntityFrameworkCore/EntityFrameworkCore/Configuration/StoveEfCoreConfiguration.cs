using System;
using System.ComponentModel;

using Autofac.Extras.IocManager;

using Microsoft.EntityFrameworkCore;

namespace Stove.EntityFrameworkCore.Configuration
{
	public class StoveEfCoreConfiguration : IStoveEfCoreConfiguration, ISingletonDependency
	{
		//private readonly IIocManager _iocManager;

		//public StoveEfCoreConfiguration(IIocManager iocManager)
		//{
		//	_iocManager = iocManager;
		//}

		public void AddDbContext<TDbContext>(Action<StoveDbContextConfiguration<TDbContext>> action)
			where TDbContext : DbContext
		{
			//_iocManager.IocContainer.Register(
			//    Component.For<IStoveDbContextConfigurer<TDbContext>>().Instance(
			//        new StoveDbContextConfigurerAction<TDbContext>(action)
			//    ).IsDefault()
			//);
		}
	}
}