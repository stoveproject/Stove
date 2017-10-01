using System;

using Autofac;
using Autofac.Extras.IocManager;

using Raven.Client.Documents;

using Stove.Domain.Repositories;
using Stove.RavenDB.Configuration;
using Stove.RavenDB.Repositories;
using Stove.Reflection.Extensions;

namespace Stove.RavenDB
{
	public static class StoveRavenDBRegistrationExtensions
	{
		public static IIocBuilder UseStoveRavenDB(this IIocBuilder builder, Func<IStoveRavenDBConfiguration, IStoveRavenDBConfiguration> configurer)
		{
			return builder.RegisterServices(r =>
			{
				r.RegisterAssemblyByConvention(typeof(StoveRavenDBRegistrationExtensions).GetAssembly());
				r.UseBuilder(cb =>
				{
					cb.RegisterGeneric(typeof(RavenDBRepositoryBase<>)).As(typeof(IRepository<>)).WithPropertyInjection(false);
					cb.RegisterGeneric(typeof(RavenDBRepositoryBase<,>)).As(typeof(IRepository<,>)).WithPropertyInjection(false);
				});
				r.Register(ctx => configurer);
				r.Register<IDocumentStore>(ctx =>
				{
					var cfg = ctx.Resolver.Resolve<IStoveRavenDBConfiguration>();

					return new DocumentStore
					{
						Urls = cfg.Urls,
						Database = cfg.DefaultDatabase
					};
				}, Lifetime.Singleton);
			});
		}
	}
}
