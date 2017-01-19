using System;

using Autofac.Extras.IocManager;

namespace Stove.Dependency
{
    public static class ResolverExtensions
    {
        public static bool ResolveIfExists<T>(this IResolver resolver, out T instance) where T : class
        {
            instance = null;
            if (resolver.IsRegistered<T>())
            {
                instance = resolver.Resolve<T>();
                return true;
            }

            return false;
        }

        public static bool ResolveIfExists(this IResolver resolver, Type type, out object instance)
        {
            instance = null;
            if (resolver.IsRegistered(type))
            {
                instance = resolver.Resolve(type);
                return true;
            }

            return false;
        }
    }
}
