using System;
using System.Reflection;

using Mapster;

namespace Stove.Mapster.Mapster
{
    internal static class AutoMapperConfigurationExtensions
    {
        public static void CreateAutoAttributeMaps(this TypeAdapterConfig configuration, Type type)
        {
            foreach (AutoMapAttributeBase autoMapAttribute in type.GetCustomAttributes<AutoMapAttributeBase>())
            {
                autoMapAttribute.CreateMap(configuration, type);
            }
        }
    }
}
