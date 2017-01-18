using System;
using System.Reflection;

using Mapster;

using Stove.Collections.Extensions;

namespace Stove.Mapster.Mapster
{
    public class AutoMapAttribute : AutoMapAttributeBase
    {
        public AutoMapAttribute(params Type[] targetTypes)
            : base(targetTypes)
        {
        }

        public override void CreateMap(TypeAdapterConfig configuration, Type destination)
        {
            if (TargetTypes.IsNullOrEmpty())
            {
                return;
            }

            foreach (Type source in TargetTypes)
            {
                MethodInfo mapToDestination = configuration.GetType().GetMethod("NewConfig").MakeGenericMethod(source, destination);
                MethodInfo mapToSource = configuration.GetType().GetMethod("NewConfig").MakeGenericMethod(destination, source);
                mapToDestination.Invoke(configuration, null);
                mapToSource.Invoke(configuration, null);
            }
        }
    }
}
