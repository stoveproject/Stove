using System;

using Mapster;

namespace Stove.Mapster.Mapster
{
    public abstract class AutoMapAttributeBase : Attribute
    {
        protected AutoMapAttributeBase(params Type[] targetTypes)
        {
            TargetTypes = targetTypes;
        }

        public Type[] TargetTypes { get; private set; }

        public abstract void CreateMap(TypeAdapterConfig configuration, Type destination);
    }
}
