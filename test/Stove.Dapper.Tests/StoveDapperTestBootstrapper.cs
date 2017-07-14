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
            Configuration.DefaultNameOrConnectionString = connectionString;
            Configuration.TypedConnectionStrings.Add(typeof(DapperAppTestStoveDbContext), Configuration.DefaultNameOrConnectionString);
            Configuration.TypedConnectionStrings.Add(typeof(MailDbContext), Configuration.DefaultNameOrConnectionString);

            Configuration.UnitOfWork.IsolationLevel = IsolationLevel.Unspecified;
        }

        public override void Start()
        {
            DapperExtensions.DapperExtensions.SqlDialect = new SqliteDialect();
            DapperExtensions.DapperExtensions.SetMappingAssemblies(new List<Assembly> { Assembly.GetExecutingAssembly() });
        }
    }
}
