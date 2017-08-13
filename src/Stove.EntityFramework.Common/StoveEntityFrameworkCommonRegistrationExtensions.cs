using Autofac.Extras.IocManager;

using Stove.Reflection.Extensions;

namespace Stove.EntityFramework.Common
{
	public static class StoveEntityFrameworkCommonRegistrationExtensions
	{
		public static IIocBuilder UseStoveEntityFrameworkCommon(this IIocBuilder builder)
		{
			return builder
				.RegisterServices(r => r.RegisterAssemblyByConvention(typeof(StoveEntityFrameworkCommonRegistrationExtensions).GetAssembly()));
		}
	}
}
