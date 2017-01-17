using System.Configuration;

using Stove.Configuration;
using Stove.Extensions;

namespace Stove.Redis.Runtime.Caching.Redis
{
    public class StoveRedisCacheOptions
    {
        private const string ConnectionStringKey = "Stove.Redis.Cache";
        private const string DatabaseIdSettingKey = "Stove.Redis.Cache.DatabaseId";

        public StoveRedisCacheOptions(IStoveStartupConfiguration stoveStartupConfiguration)
        {
            StoveStartupConfiguration = stoveStartupConfiguration;
            ConnectionString = GetDefaultConnectionString();
            DatabaseId = GetDefaultDatabaseId();
        }

        public IStoveStartupConfiguration StoveStartupConfiguration { get; private set; }

        public string ConnectionString { get; set; }

        public int DatabaseId { get; set; }

        private static int GetDefaultDatabaseId()
        {
            string appSetting = ConfigurationManager.AppSettings[DatabaseIdSettingKey];
            if (appSetting.IsNullOrEmpty())
            {
                return -1;
            }

            int databaseId;
            if (!int.TryParse(appSetting, out databaseId))
            {
                return -1;
            }

            return databaseId;
        }

        private static string GetDefaultConnectionString()
        {
            ConnectionStringSettings connStr = ConfigurationManager.ConnectionStrings[ConnectionStringKey];
            if (connStr == null || connStr.ConnectionString.IsNullOrWhiteSpace())
            {
                return "localhost";
            }

            return connStr.ConnectionString;
        }
    }
}
