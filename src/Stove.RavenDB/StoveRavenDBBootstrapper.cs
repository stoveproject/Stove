using Raven.Client;

using Stove.Bootstrapping;
using Stove.RavenDB.Configuration;

namespace Stove
{
    [DependsOn(
        typeof(StoveKernelBootstrapper)
    )]
    public class StoveRavenDBBootstrapper : StoveBootstrapper
    {
        public override void PreStart()
        {
            Configuration.GetConfigurerIfExists<IStoveRavenDBConfiguration>()(Configuration.Modules.StoveRavenDB());
        }

        public override void Start()
        {
            Configuration.Resolver.Resolve<IDocumentStore>().Initialize();
        }

        public override void Shutdown()
        {
            Configuration.Resolver.Resolve<IDocumentStore>().Dispose();
        }
    }
}
