using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

using Dapper;

using Stove.TestBase;

namespace Stove.Dapper.Tests
{
    public abstract class StoveDapperApplicationTestBase : ApplicationTestBase<StoveDapperTestBootstrapper>
    {
        private readonly string _connectionString;

        protected StoveDapperApplicationTestBase()
        {
            Building(builder =>
            {
                builder
                    .UseStoveEntityFramework()
                    .UseStoveDapper()
                    .UseStoveEventBus()
                    .UseStoveTypedConnectionStringResolver()
                    .UseStoveDbContextEfTransactionStrategy();

                builder.RegisterServices(r => r.Register<DbConnection>(ctx =>
                {
                    var connection = new SqlConnection(_connectionString);

                    var files = new List<string>
                    {
                        ReadScriptFile("CreateInitialTables")
                    };

                    foreach (string setupFile in files)
                    {
                        connection.Execute(setupFile);
                    }

                    return connection;
                }));

                builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
            });

            string executable = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(executable))) + @"\Db\StoveDapperTest.mdf";
            _connectionString = $@"Data Source=(localdb)\MsSqlLocalDb;Integrated Security=SSPI;AttachDBFilename={path}";
        }

        protected override void PostBuild()
        {
            base.PostBuild();

            StoveSession.UserId = 1;
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
