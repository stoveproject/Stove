using System;

using Stove.Configuration;
using Stove.Domain.Uow;

namespace Stove.EntityFramework.Uow
{
    public class TypedConnectionStringResolver : IConnectionStringResolver
    {
        private readonly IStoveStartupConfiguration _configuration;

        public TypedConnectionStringResolver(IStoveStartupConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetNameOrConnectionString(ConnectionStringResolveArgs args)
        {
            string connectionString;
            if (_configuration.TypedConnectionStrings.TryGetValue((Type)args["DbContextType"], out connectionString))
            {
                return connectionString;
            }

            throw new StoveException("Could not find DbContextType to resolve connection string!");
        }
    }
}
