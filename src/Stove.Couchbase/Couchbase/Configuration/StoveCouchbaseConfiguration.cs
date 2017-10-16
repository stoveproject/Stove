using Autofac.Extras.IocManager;

using Couchbase.Configuration.Client;

namespace Stove.Couchbase.Configuration
{
    public class StoveCouchbaseConfiguration : IStoveCouchbaseConfiguration, ISingletonDependency
    {
        public StoveCouchbaseConfiguration()
        {
            ClientConfiguration = new ClientConfiguration();
        }

        public ClientConfiguration ClientConfiguration { get; set; }
    }
}
