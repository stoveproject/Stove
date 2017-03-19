using System;
using System.Reflection;

using Mapster;

using Stove.Collections.Extensions;

namespace Stove.Mapster
{
    /// <summary>
    ///     Maps both, eneity and dto two-way.
    /// </summary>
    /// <seealso cref="AutoMapAttributeBase" />
    public class AutoMapAttribute : AutoMapAttributeBase
    {
        public AutoMapAttribute(params Type[] targetTypes)
            : base(targetTypes)
        {
        }

        public override void CreateMap(TypeAdapterConfig configuration, Type needstoMap)
        {
            if (TargetTypes.IsNullOrEmpty())
            {
                return;
            }

            foreach (Type source in TargetTypes)
            {
                MethodInfo mapToDestination = configuration.GetType().GetMethod("NewConfig").MakeGenericMethod(source, needstoMap);
                MethodInfo mapToSource = configuration.GetType().GetMethod("NewConfig").MakeGenericMethod(needstoMap, source);
                mapToDestination.Invoke(configuration, null);
                mapToSource.Invoke(configuration, null);
            }
        }
    }
}
