using Stove.Configuration;

namespace Stove.EntityFrameworkCore.Configuration
{
	/// <summary>
	///     Defines extension methods to <see cref="IModuleConfigurations" /> to allow to configure Stove EntityFramework Core
	///     module.
	/// </summary>
	public static class StoveEfCoreConfigurationExtensions
	{
		/// <summary>
		///     Used to configure Stove EntityFramework Core module.
		/// </summary>
		public static IStoveEfCoreConfiguration StoveEfCore(this IModuleConfigurations configurations)
		{
			return configurations.StoveConfiguration.Get<IStoveEfCoreConfiguration>();
		}
	}
}
