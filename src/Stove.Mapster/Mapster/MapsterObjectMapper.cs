using Mapster;

using Stove.ObjectMapping;

namespace Stove.Mapster.Mapster
{
    public class MapsterObjectMapper : IObjectMapper
    {
        public TDestination Map<TDestination>(object source)
        {
            return source.Adapt<TDestination>();
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return source.Adapt(destination);
        }
    }
}
