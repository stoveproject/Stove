using Autofac.Extras.IocManager;

using JetBrains.Annotations;

using Stove.Domain.Uow;
using Stove.EntityFramework.Uow;
using Stove.Reflection.Extensions;

namespace Stove.EntityFramework
{
	public static class StoveEntityFrameworkCommonRegistrationExtensions
	{
		public static IIocBuilder UseStoveEntityFrameworkCommon(this IIocBuilder builder)
		{
			return builder
				.RegisterServices(r => r.RegisterAssemblyByConvention(typeof(StoveEntityFrameworkCommonRegistrationExtensions).GetAssembly()));
		}

		/// <summary>
		///     Uses the stove typed connection string resolver.
		/// </summary>
		/// <param name="builder">The builder.</param>
		/// <returns></returns>
		[NotNull]
		public static IIocBuilder UseStoveTypedConnectionStringResolver([NotNull] this IIocBuilder builder)
		{
			return builder.RegisterServices(r => r.Register<IConnectionStringResolver, TypedConnectionStringResolver>());
		}
	}
}
