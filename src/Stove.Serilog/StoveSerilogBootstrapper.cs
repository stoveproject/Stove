using Stove.Bootstrapping;

namespace Stove
{
	[DependsOn(
		typeof(StoveKernelBootstrapper)
	)]
	public class StoveSerilogBootstrapper : StoveBootstrapper
	{
	}
}
