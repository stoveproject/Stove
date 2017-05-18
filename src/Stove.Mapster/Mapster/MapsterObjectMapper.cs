using System;

using Mapster;

using Stove.ObjectMapping;

namespace Stove.Mapster
{
    public class MapsterObjectMapper : IObjectMapper
    {
        private readonly IStoveMapsterConfiguration _mapsterConfiguration;

        public MapsterObjectMapper(IStoveMapsterConfiguration mapsterConfiguration)
        {
            _mapsterConfiguration = mapsterConfiguration;
        }

        public TDestination Map<TDestination>(object source)
        {
            Type sourceType = source.GetType().Namespace == "System.Data.Entity.DynamicProxies"
                ? source.GetType().BaseType
                : source.GetType();

            return (TDestination)source.Adapt(sourceType, typeof(TDestination), _mapsterConfiguration.Configuration);
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return source.Adapt(destination, _mapsterConfiguration.Configuration);
        }
    }
}
