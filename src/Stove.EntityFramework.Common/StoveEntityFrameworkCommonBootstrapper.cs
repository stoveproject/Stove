using Stove.Bootstrapping;

namespace Stove.EntityFramework
{
	[DependsOn(
		typeof(StoveKernelBootstrapper)
		)]
	public class StoveEntityFrameworkCommonBootstrapper : StoveBootstrapper
	{
	}
}
