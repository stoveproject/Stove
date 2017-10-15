using Stove.Bootstrapping;
using Stove.Couchbase.Couchbase;

namespace Stove.Couchbase.Tests
{
    [DependsOn(
        typeof(StoveCouchbaseBootstrapper)
    )]
    public class CouchbaseTestBootstrapper : StoveBootstrapper
    {
    }
}
