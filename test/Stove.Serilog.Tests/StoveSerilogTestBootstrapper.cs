using Stove.Bootstrapping;

namespace Stove.NLog.Tests
{
	[DependsOn(
		typeof(StoveSerilogBootstrapper)
	)]
	public class StoveSerilogTestBootstrapper : StoveBootstrapper
	{
	}
}
