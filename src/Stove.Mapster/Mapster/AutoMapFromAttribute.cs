using System;
using System.Reflection;

using Mapster;

using Stove.Collections.Extensions;

namespace Stove.Mapster
{
    /// <summary>
    ///     From Entity to Dto, Use on Entities.
    /// </summary>
    /// <seealso cref="AutoMapAttributeBase" />
    public class AutoMapFromAttribute : AutoMapAttributeBase
    {
        public AutoMapFromAttribute(params Type[] targetTypes)
            : base(targetTypes)
        {
        }

        public override void CreateMap(TypeAdapterConfig configuration, Type destination)
        {
            if (TargetTypes.IsNullOrEmpty())
            {
                return;
            }

            foreach (Type target in TargetTypes)
            {
                configuration.NewConfig(target, destination);
            }
        }
    }
}
