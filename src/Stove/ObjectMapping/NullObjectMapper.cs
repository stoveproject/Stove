using Autofac.Extras.IocManager;

namespace Stove.ObjectMapping
{
	public sealed class NullObjectMapper : IObjectMapper, ISingletonDependency
	{
		/// <summary>
		///     Singleton instance.
		/// </summary>
		public static NullObjectMapper Instance { get; } = new NullObjectMapper();

		public TDestination Map<TDestination>(object source)
		{
			throw new StoveException("Stove.ObjectMapping.IObjectMapper should be implemented in order to map objects.");
		}

		public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
		{
			throw new StoveException("Stove.ObjectMapping.IObjectMapper should be implemented in order to map objects.");
		}
	}
}
