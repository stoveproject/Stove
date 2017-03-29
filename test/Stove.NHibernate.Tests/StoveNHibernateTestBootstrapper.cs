using System.Collections.Generic;
using System.Reflection;

using DapperExtensions.Sql;

using Stove.Bootstrapping;

namespace Stove.NHibernate.Tests
{
    [DependsOn(
        typeof(StoveNHibernateBootstrapper)
    )]
    public class StoveNHibernateTestBootstrapper : StoveBootstrapper
    {
        public override void PreStart()
        { 
            DapperExtensions.DapperExtensions.SqlDialect = new SqliteDialect();
            DapperExtensions.DapperExtensions.SetMappingAssemblies(new List<Assembly> { Assembly.GetExecutingAssembly() });
        }
    }
}
