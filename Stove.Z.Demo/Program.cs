using System.Configuration;

using Autofac.Extras.IocManager;

using Stove.Configuration;
using Stove.EntityFramework;

namespace Stove.Z.Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            IRootResolver resolver = IocBuilder.New
                                               .UseAutofacContainerBuilder()
                                               .UseStove()
                                               .UseEntityFramework()
                                               .UseDefaultEventBus()
                                               .UseDbContextEfTransactionStrategy()
                                               .RegisterServices(r => r.Register(context =>
                                               {
                                                   var configuration = context.Resolver.Resolve<IStoveStartupConfiguration>();
                                                   configuration.DefaultNameOrConnectionString = ConfigurationManager.ConnectionStrings["DemoDbContext"].ToString();
                                                   return configuration;
                                               }))
                                               .CreateResolver();
        }
    }
}
