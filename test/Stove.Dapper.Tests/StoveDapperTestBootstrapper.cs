using System.Collections.Generic;
using System.Reflection;
using System.Transactions;

using DapperExtensions.Sql;

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
            var connectionString = "Data Source=:memory:";
            StoveConfiguration.DefaultNameOrConnectionString = connectionString;
            StoveConfiguration.TypedConnectionStrings.Add(typeof(DapperAppTestStoveDbContext), StoveConfiguration.DefaultNameOrConnectionString);
            StoveConfiguration.TypedConnectionStrings.Add(typeof(MailDbContext), StoveConfiguration.DefaultNameOrConnectionString);

            StoveConfiguration.UnitOfWork.IsolationLevel = IsolationLevel.Unspecified;
        }

        public override void Start()
        {
            DapperExtensions.DapperExtensions.SqlDialect = new SqliteDialect();
            DapperExtensions.DapperExtensions.SetMappingAssemblies(new List<Assembly> { Assembly.GetExecutingAssembly() });
        }
    }
}
