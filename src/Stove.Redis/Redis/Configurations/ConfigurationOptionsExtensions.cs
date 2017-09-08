using System;

using StackExchange.Redis;

namespace Stove.Redis.Configurations
{
    public static class ConfigurationOptionsExtensions
    {
        /// <summary>
        ///     Adds the endpoint.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="port">The port.</param>
        /// <returns></returns>
        public static ConfigurationOptions AddEndpoint(this ConfigurationOptions options, string endpoint, int port = 6379)
        {
            options.EndPoints.Add(endpoint, port);
            return options;
        }

        /// <summary>
        ///     Sets the default database.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="defaultDatabaseId">The default database identifier.</param>
        /// <returns></returns>
        public static ConfigurationOptions SetDefaultDatabase(this ConfigurationOptions options, int defaultDatabaseId)
        {
            options.DefaultDatabase = defaultDatabaseId;
            return options;
        }

        /// <summary>
        ///     Sets the connection time out.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="connectionTimeout">The connection timeout.</param>
        /// <returns></returns>
        public static ConfigurationOptions SetConnectionTimeOut(this ConfigurationOptions options, TimeSpan connectionTimeout)
        {
            options.ConnectTimeout = (int)connectionTimeout.TotalMilliseconds;
            return options;
        }

        /// <summary>
        ///     Allows to make Secure Sockets Layer (SSL) connection
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns></returns>
        public static ConfigurationOptions AllowSsl(this ConfigurationOptions options)
        {
            options.Ssl = true;
            return options;
        }
    }
}
