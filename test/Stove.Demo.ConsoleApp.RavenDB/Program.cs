using System.Reflection;

using Autofac.Extras.IocManager;

namespace Stove.Demo.ConsoleApp.RavenDB
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IRootResolver resolver = IocBuilder.New
                                               .UseAutofacContainerBuilder()
                                               .UseStoveWithNullables<StoveRavenDBDemoBootstrapper>()
                                               .UseStoveRavenDB(cfg =>
                                               {
                                                   cfg.DefaultDatabase = "Stove";
                                                   cfg.Url = "http://localhost:8080/";
                                                   return cfg;
                                               })
                                               .RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()))
                                               .CreateResolver();

            using (resolver)
            {
                var service = resolver.Resolve<RavenDomainService>();
                service.DoSomeStuff();
            }
        }
    }
}
