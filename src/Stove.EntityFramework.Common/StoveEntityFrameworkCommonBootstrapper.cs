using Stove.Bootstrapping;

namespace Stove.EntityFramework.Common
{
	[DependsOn(
		typeof(StoveKernelBootstrapper)
		)]
	public class StoveEntityFrameworkCommonBootstrapper : StoveBootstrapper
	{
	}
}
