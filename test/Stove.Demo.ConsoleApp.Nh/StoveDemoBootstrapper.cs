using System.Configuration;

using Stove.Bootstrapping;
using Stove.Dapper;
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
        }
    }
}
