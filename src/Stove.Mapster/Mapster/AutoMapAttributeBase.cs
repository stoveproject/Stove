using System;

using JetBrains.Annotations;

using Mapster;

namespace Stove.Mapster
{
    public abstract class AutoMapAttributeBase : Attribute
    {
        protected AutoMapAttributeBase([NotNull] params Type[] targetTypes)
        {
            TargetTypes = targetTypes;
        }

        [NotNull]
        public Type[] TargetTypes { get; private set; }

        public abstract void CreateMap([NotNull] TypeAdapterConfig configuration, [NotNull] Type needstoMap);
    }
}
