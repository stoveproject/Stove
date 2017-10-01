using System.Reflection;

using Autofac.Extras.IocManager;

using Stove.RavenDB;

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
				                                   cfg.Urls = new string[] { "http://localhost:8080/" };
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
