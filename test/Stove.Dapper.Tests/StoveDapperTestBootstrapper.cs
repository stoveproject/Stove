using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

using Dapper;

using Stove.Bootstrapping;
using Stove.Dapper.Tests.DbContexes;

namespace Stove.Dapper.Tests
{
    [DependsOn(
        typeof(StoveEntityFrameworkBootstrapper),
        typeof(StoveDapperBootstrapper)
    )]
    public class StoveDapperTestBootstrapper : StoveBootstrapper
    {
        public override void PreStart()
        {
            string executable = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(executable))) + @"\Db\StoveDapperTest.mdf";
            string connectionString = $@"Data Source=(localdb)\MsSqlLocalDb;Integrated Security=SSPI;AttachDBFilename={path}";

            Configuration.DefaultNameOrConnectionString = connectionString;
            Configuration.TypedConnectionStrings.Add(typeof(SampleDapperApplicationDbContext), Configuration.DefaultNameOrConnectionString);
            Configuration.TypedConnectionStrings.Add(typeof(MailDbContext), Configuration.DefaultNameOrConnectionString);
        }

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
