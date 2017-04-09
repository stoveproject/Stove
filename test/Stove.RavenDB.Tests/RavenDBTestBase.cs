using System.Reflection;

using Autofac.Extras.IocManager;

using Raven.Client;
using Raven.Client.Embedded;

using Stove.RavenDB.Configuration;
using Stove.TestBase;

namespace Stove.RavenDB.Tests
{
    public abstract class RavenDBTestBase : ApplicationTestBase<RavenDBTestBootstrapper>
    {
        protected RavenDBTestBase():base(true)
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
                        r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
                        r.Register<IDocumentStore>(ctx =>
                        {
                            var configuration = ctx.Resolver.Resolve<IStoveRavenDBConfiguration>();
                            var store = new EmbeddableDocumentStore();
                            store.Conventions.AllowQueriesOnId = configuration.AllowQueriesOnId;
                            return store;
                        }, Lifetime.Singleton);
                    });
            });
        }
    }
}
