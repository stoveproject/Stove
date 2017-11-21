using System.Collections.Generic;
using System.Reflection;

using DapperExtensions.Sql;

using Stove.Bootstrapping;
using Stove.NHibernate.Tests.Sessions;

namespace Stove.NHibernate.Tests
{
    [DependsOn(
        typeof(StoveNHibernateBootstrapper)
    )]
    public class StoveNHibernateTestBootstrapper : StoveBootstrapper
    {
        public override void PreStart()
        {
            StoveConfiguration.DefaultNameOrConnectionString = "data source=:memory:";
            StoveConfiguration.TypedConnectionStrings.Add(typeof(PrimaryStoveSessionContext), StoveConfiguration.DefaultNameOrConnectionString);
            StoveConfiguration.TypedConnectionStrings.Add(typeof(SecondaryStoveSessionContext), StoveConfiguration.DefaultNameOrConnectionString);

            DapperExtensions.DapperExtensions.SqlDialect = new SqliteDialect();
            DapperExtensions.DapperExtensions.SetMappingAssemblies(new List<Assembly> { Assembly.GetExecutingAssembly() });
        }
    }
}
