using Autofac.Extras.IocManager;

using Raven.Client.Documents;
using Raven.Client.Embedded;

using Stove.RavenDB.Configuration;
using Stove.Reflection.Extensions;
using Stove.TestBase;

namespace Stove.RavenDB.Tests
{
	public abstract class RavenDBTestBase : ApplicationTestBase<RavenDBTestBootstrapper>
	{
		protected RavenDBTestBase() : base(true)
		{
			Building(builder =>
			{
				builder
					.UseStoveRavenDB(configuration =>
					{
						configuration.AllowQueriesOnId = false;
						return configuration;
					})
					.RegisterServices(r =>
					{
						r.RegisterAssemblyByConvention(typeof(RavenDBTestBase).GetAssembly());
						r.Register<IDocumentStore>(ctx =>
						{
							var configuration = ctx.Resolver.Resolve<IStoveRavenDBConfiguration>();
							var store = new EmbeddableDocumentStore
							{
								RunInMemory = true
							};
							store.Configuration.Storage.Voron.AllowOn32Bits = true;
							store.Conventions.AllowQueriesOnId = configuration.AllowQueriesOnId;
							return store;

						}, Lifetime.Singleton);
					});
			});
		}
	}
}
