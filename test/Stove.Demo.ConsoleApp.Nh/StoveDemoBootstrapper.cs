using System.Configuration;

using Stove.Bootstrapping;
using Stove.Dapper;
using Stove.Demo.ConsoleApp.Nh.SessionContexts;
using Stove.NHibernate;

namespace Stove.Demo.ConsoleApp.Nh
{
    [DependsOn(
        typeof(StoveNHibernateBootstrapper),
        typeof(StoveDapperBootstrapper)
    )]
    public class StoveDemoBootstrapper : StoveBootstrapper
    {
        public override void PreStart()
        {
            StoveConfiguration.DefaultNameOrConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            StoveConfiguration.TypedConnectionStrings.Add(typeof(PrimarySessionContext), StoveConfiguration.DefaultNameOrConnectionString);
            StoveConfiguration.TypedConnectionStrings.Add(typeof(SecondarySessionContext), StoveConfiguration.DefaultNameOrConnectionString);
        }
    }
}
