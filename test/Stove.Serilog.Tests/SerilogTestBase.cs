using Serilog;

using Stove.Reflection.Extensions;
using Stove.TestBase;

namespace Stove.NLog.Tests
{
	public abstract class SerilogTestBase : ApplicationTestBase<StoveSerilogTestBootstrapper>
	{
		protected SerilogTestBase()
		{
			Building(builder =>
			{
				builder.UseStoveSerilog(loggerConfiguration =>
				{
					loggerConfiguration
						.WriteTo.Trace()
						.MinimumLevel.Debug();
				});
				builder.RegisterServices(r => r.RegisterAssemblyByConvention(typeof(SerilogTestBase).GetAssembly()));
			});
		}
	}
}
