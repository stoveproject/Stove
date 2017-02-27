using System;

using JetBrains.Annotations;

namespace Stove.Bootstrapping
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependsOnAttribute : Attribute
    {
        public DependsOnAttribute([NotNull] params Type[] dependedBootstrapperTypes)
        {
            DependedBootstrapperTypes = dependedBootstrapperTypes;
        }

        [NotNull]
        public Type[] DependedBootstrapperTypes { get; private set; }
    }
}
