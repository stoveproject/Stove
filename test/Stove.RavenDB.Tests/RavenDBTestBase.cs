using System.Reflection;

using Autofac.Extras.IocManager;

using Raven.Client;
using Raven.Client.Embedded;

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
                    .UseStoveRavenDB(configuration => { return configuration; })
                    .RegisterServices(r =>
                    {
                        r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
                        r.Register<IDocumentStore>(ctx => new EmbeddableDocumentStore(), Lifetime.Singleton);
                    });
            });
        }
    }
}
