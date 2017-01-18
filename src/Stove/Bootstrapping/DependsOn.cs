using System;

namespace Stove.Bootstrapping
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class DependsOnAttribute : Attribute
    {
        public DependsOnAttribute(params Type[] dependedBootstrapperTypes)
        {
            DependedBootstrapperTypes = dependedBootstrapperTypes;
        }

        public Type[] DependedBootstrapperTypes { get; private set; }
    }
}
