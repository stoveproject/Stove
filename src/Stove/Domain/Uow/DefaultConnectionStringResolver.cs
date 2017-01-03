using System;
using System.Configuration;

using Stove.Configuration;

namespace Stove.Domain.Uow
{
    /// <summary>
    ///     Default implementation of <see cref="IConnectionStringResolver" />.
    ///     Get connection string from <see cref="IStoveStartupConfiguration" />,
    ///     or "Default" connection string in config file,
    ///     or single connection string in config file.
    /// </summary>
    public class DefaultConnectionStringResolver : IConnectionStringResolver
    {
        private readonly IStoveStartupConfiguration _configuration;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DefaultConnectionStringResolver" /> class.
        /// </summary>
        public DefaultConnectionStringResolver(IStoveStartupConfiguration configuration)
        {
            _configuration = configuration;
        }

        public virtual string GetNameOrConnectionString(ConnectionStringResolveArgs args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            string defaultConnectionString = _configuration.DefaultNameOrConnectionString;
            if (!string.IsNullOrWhiteSpace(defaultConnectionString))
            {
                return defaultConnectionString;
            }

            if (ConfigurationManager.ConnectionStrings["Default"] != null)
            {
                return "Default";
            }

            if (ConfigurationManager.ConnectionStrings.Count == 1)
            {
                return ConfigurationManager.ConnectionStrings[0].ConnectionString;
            }

            throw new StoveException("Could not find a connection string definition for the application. Set IStoveStartupConfiguration.DefaultNameOrConnectionString or add a 'Default' connection string to application .config file.");
        }
    }
}
