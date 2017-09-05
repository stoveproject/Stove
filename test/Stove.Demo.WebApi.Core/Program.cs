using System.IO;

using Microsoft.AspNetCore.Hosting;

namespace Stove.Demo.WebApi.Core
{
	public class Program
	{
		public static void Main(string[] args)
		{
			IWebHost host = new WebHostBuilder()
				.UseKestrel()
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseStartup<Startup>()
				.UseIISIntegration()
				.UseApplicationInsights()
				.Build();

			host.Run();
		}
	}
}
