using Couchbase.Configuration.Client;

namespace Stove.Couchbase.Configuration
{
    public interface IStoveCouchbaseConfiguration
    {
        ClientConfiguration ClientConfiguration { get; set; }
    }
}
