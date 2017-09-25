using Autofac.Extras.IocManager;

using Stove.Configuration;

namespace Stove.RavenDB.Configuration
{
	public class StoveRavenDBConfiguration : IStoveRavenDBConfiguration, ISingletonDependency
	{
		public StoveRavenDBConfiguration(IStoveStartupConfiguration configuration)
		{
			Configuration = configuration;
		}

		public string[] Urls { get; set; }

		public string DefaultDatabase { get; set; }

		public bool AllowQueriesOnId { get; set; }

		public IStoveStartupConfiguration Configuration { get; }
	}
}
