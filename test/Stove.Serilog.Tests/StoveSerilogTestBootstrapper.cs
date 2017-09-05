using Stove.Bootstrapping;

namespace Stove.Serilog.Tests
{
	[DependsOn(
		typeof(StoveSerilogBootstrapper)
	)]
	public class StoveSerilogTestBootstrapper : StoveBootstrapper
	{
	}
}
