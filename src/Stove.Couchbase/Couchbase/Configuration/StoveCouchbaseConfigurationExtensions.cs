using Stove.Configuration;

namespace Stove.Couchbase.Couchbase.Configuration
{
    public static class StoveCouchbaseConfigurationExtensions
    {
        public static IStoveCouchbaseConfiguration StoveCouchbase(this IModuleConfigurations modules)
        {
            return modules.StoveConfiguration.Get<IStoveCouchbaseConfiguration>();
        }
    }
}
