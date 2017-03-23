using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

using Dapper;

using Stove.Bootstrapping;

namespace Stove.Dapper.Tests
{
    [DependsOn(
        typeof(StoveEntityFrameworkBootstrapper),
        typeof(StoveDapperBootstrapper)
    )]
    public class StoveDapperTestBootstrapper : StoveBootstrapper
    {
        public override void Start()
        {
            DapperExtensions.DapperExtensions.SetMappingAssemblies(new List<Assembly> { Assembly.GetExecutingAssembly() });
        }

        public override void Shutdown()
        {
            var connection = new SqlConnection(Configuration.DefaultNameOrConnectionString);

            var files = new List<string>
            {
                ReadScriptFile("DestroyScript")
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
