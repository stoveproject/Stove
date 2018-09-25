using System;
using Autofac.Extras.IocManager;

using Raven.Client.Documents;
using Raven.Embedded;
using Raven.TestDriver;

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
					.UseStoveRavenDB(configuration => { return configuration; })
					.RegisterServices(r =>
					{
						r.RegisterAssemblyByConvention(typeof(RavenDBTestBase).GetAssembly());
						r.Register(ctx =>
						{
							IDocumentStore store = EmbeddedServer.Instance.GetDocumentStore(Guid.NewGuid().ToString());

							return store;
						}, Lifetime.Singleton);
					});
			});
		}
	}
}
