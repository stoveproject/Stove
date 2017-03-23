using System;
using System.Configuration;
using System.Reflection;

using Autofac.Extras.IocManager;

using FluentNHibernate.Cfg.Db;

using HibernatingRhinos.Profiler.Appender.NHibernate;

namespace Stove.Demo.ConsoleApp.Nh
{
    public class Program
    {
        public static void Main(string[] args)
        {
            NHibernateProfiler.Initialize();

            string connString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

            IRootResolver rootResolver = IocBuilder.New
                                                   .UseAutofacContainerBuilder()
                                                   .UseStove<StoveDemoBootstrapper>()
                                                   .UseStoveNullLogger()
                                                   .UseStoveNHibernate(nhCfg =>
                                                   {
                                                       nhCfg.FluentConfiguration
                                                            .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connString))
                                                            .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()));

                                                       return nhCfg;
                                                   })
                                                   .UseStoveDefaultConnectionStringResolver()
                                                   .UseStoveDapper()
                                                   .UseStoveEventBus()
                                                   .RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()))
                                                   .CreateResolver();

            using (rootResolver)
            {
                rootResolver.Resolve<ProductDomainService>().DoSomeCoolStuff();
            }

            NHibernateProfiler.Shutdown();

            Console.ReadLine();
        }
    }
}
