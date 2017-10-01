using Stove.Bootstrapping;

namespace Stove.Serilog
{
	[DependsOn(
		typeof(StoveKernelBootstrapper)
	)]
	public class StoveSerilogBootstrapper : StoveBootstrapper
	{
	}
}
