using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Interception;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

using Dapper;

using Stove.Bootstrapping;
using Stove.Demo.ConsoleApp.DbContexes;
using Stove.EntityFramework.Interceptors;

namespace Stove.Demo.ConsoleApp
{
    [DependsOn(
        typeof(StoveEntityFrameworkBootstrapper),
        typeof(StoveMapsterBootstrapper),
        typeof(StoveDapperBootstrapper),
		typeof(StoveHangFireBootstrapper),
		typeof(StoveRabbitMQBootstrapper),
		typeof(StoveRedisBootstrapper)
	)]
    public class StoveDemoBootstrapper : StoveBootstrapper
    {
        public override void PreStart()
        {
	        StoveConfiguration.DefaultNameOrConnectionString = ConnectionStringHelper.GetConnectionString("Default");
	        StoveConfiguration.TypedConnectionStrings.Add(typeof(AnimalStoveDbContext), "Default");
	        StoveConfiguration.TypedConnectionStrings.Add(typeof(PersonStoveDbContext), "Default");
	        StoveConfiguration.TypedConnectionStrings.Add(typeof(PriceDbContext), "Default");

            ExecuteScript("InitializeDatabase");
        }

        public override void Start()
        {
	        StoveConfiguration.Caching.Configure(DemoCacheName.Demo, cache => { cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(1); });

            DbInterception.Add(StoveConfiguration.Resolver.Resolve<WithNoLockInterceptor>());

            DapperExtensions.DapperExtensions.SetMappingAssemblies(new List<Assembly> { Assembly.GetExecutingAssembly() });
        }

        public override void Shutdown()
        {
            ExecuteScript("DestroyDatabase");
        }

        private void ExecuteScript(string scriptName)
        {
            var connection = new SqlConnection(StoveConfiguration.DefaultNameOrConnectionString);

            var files = new List<string>
            {
                ReadScriptFile(scriptName)
            };

            foreach (string setupFile in files)
            {
                connection.Execute(setupFile);
            }
        }

        private string ReadScriptFile(string name)
        {
            string fileName = GetType().Namespace + ".Scripts" + "." + name + ".sql";
            using (Stream resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(fileName))
            {
                if (resource != null)
                {
                    using (var sr = new StreamReader(resource))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }

            return string.Empty;
        }
    }
}
