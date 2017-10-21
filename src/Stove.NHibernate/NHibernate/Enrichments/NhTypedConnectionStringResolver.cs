using System;

using Autofac.Extras.IocManager;

using Stove.Configuration;
using Stove.Domain.Uow;

namespace Stove.NHibernate.Enrichments
{
    public class NhTypedConnectionStringResolver : IConnectionStringResolver, ITransientDependency
    {
        private readonly IStoveStartupConfiguration _configuration;

        public NhTypedConnectionStringResolver(IStoveStartupConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetNameOrConnectionString(ConnectionStringResolveArgs args)
        {
            if (_configuration.TypedConnectionStrings.TryGetValue((Type)args["SessionContextType"], out string connectionString)) return connectionString;

            throw new StoveException("Could not find StoveSessionContextType to resolve connection string!");
        }
    }
}
