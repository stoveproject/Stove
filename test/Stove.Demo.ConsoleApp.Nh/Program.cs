using System;
using System.Reflection;

using Autofac.Extras.IocManager;

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;

using HibernatingRhinos.Profiler.Appender.NHibernate;

using Stove.Dapper;
using Stove.Demo.ConsoleApp.Nh.SessionContexts;
using Stove.NHibernate;

namespace Stove.Demo.ConsoleApp.Nh
{
    public class Program
    {
        public static void Main(string[] args)
        {
            NHibernateProfiler.Initialize();

            IRootResolver rootResolver = IocBuilder.New
                                                   .UseAutofacContainerBuilder()
                                                   .UseStove<StoveDemoBootstrapper>()
                                                   .UseStoveNullLogger()
                                                   .UseStoveNHibernate(nhCfg =>
                                                   {
                                                       nhCfg.AddFluentConfigurationFor<PrimarySessionContext>(() =>
                                                       {
                                                           return Fluently.Configure()
                                                                          .Database(MsSqlConfiguration.MsSql2012.ConnectionString(nhCfg.Configuration.DefaultNameOrConnectionString))
                                                                          .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()));
                                                       });

                                                       nhCfg.AddFluentConfigurationFor<SecondarySessionContext>(() =>
                                                       {
                                                           return Fluently.Configure()
                                                                          .Database(MsSqlConfiguration.MsSql2012.ConnectionString(nhCfg.Configuration.DefaultNameOrConnectionString))
                                                                          .Mappings(m => m.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()));
                                                       });

                                                       return nhCfg;
                                                   })
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
