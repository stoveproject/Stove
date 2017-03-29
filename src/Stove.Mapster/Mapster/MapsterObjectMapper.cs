using Mapster;

using Stove.ObjectMapping;

namespace Stove.Mapster
{
    public class MapsterObjectMapper : IObjectMapper
    {
        private readonly IStoveMapsterConfiguration _mapsterConfiguration;

        public MapsterObjectMapper(IStoveMapsterConfiguration mapsterConfiguration)
        {
            this._mapsterConfiguration = mapsterConfiguration;
        }

        public TDestination Map<TDestination>(object source)
        {
            return source.Adapt<TDestination>(_mapsterConfiguration.Configuration);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return source.Adapt(destination, _mapsterConfiguration.Configuration);
        }
    }
}
