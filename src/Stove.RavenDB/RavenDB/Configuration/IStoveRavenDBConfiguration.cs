using Stove.Configuration;

namespace Stove.RavenDB.Configuration
{
	public interface IStoveRavenDBConfiguration
	{
		string[] Urls { get; set; }

		string DefaultDatabase { get; set; }

		bool AllowQueriesOnId { get; set; }

		IStoveStartupConfiguration Configuration { get; }
	}
}
