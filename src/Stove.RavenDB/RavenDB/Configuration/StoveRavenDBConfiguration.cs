using Autofac.Extras.IocManager;

namespace Stove.RavenDB.Configuration
{
    public class StoveRavenDBConfiguration : IStoveRavenDBConfiguration, ISingletonDependency
    {
        public string Url { get; set; }

        public string DefaultDatabase { get; set; }

        public bool AllowQueriesOnId { get; set; }
    }
}
