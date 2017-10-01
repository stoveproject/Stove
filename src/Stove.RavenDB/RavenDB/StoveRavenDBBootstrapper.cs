using Raven.Client.Documents;

using Stove.Bootstrapping;
using Stove.RavenDB.Configuration;

namespace Stove.RavenDB
{
    [DependsOn(
        typeof(StoveKernelBootstrapper)
    )]
    public class StoveRavenDBBootstrapper : StoveBootstrapper
    {
        public override void PreStart()
        {
            StoveConfiguration.GetConfigurerIfExists<IStoveRavenDBConfiguration>()(StoveConfiguration.Modules.StoveRavenDB());
        }

        public override void Start()
        {
            Resolver.Resolve<IDocumentStore>().Initialize();
        }

        public override void Shutdown()
        {
            Resolver.Resolve<IDocumentStore>().Dispose();
        }
    }
}
