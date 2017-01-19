using System;

namespace Stove.Bootstrapping
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class StarterBootstrapper : Attribute
    {
    }
}
