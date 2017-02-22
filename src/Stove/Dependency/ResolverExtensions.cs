using System;

using Autofac.Extras.IocManager;

using JetBrains.Annotations;

namespace Stove.Dependency
{
    public static class ResolverExtensions
    {
        /// <summary>
        ///     Resolves if exists.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resolver">The resolver.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static bool ResolveIfExists<T>([NotNull] this IResolver resolver, [CanBeNull] out T instance) where T : class
        {
            instance = null;
            if (resolver.IsRegistered<T>())
            {
                instance = resolver.Resolve<T>();
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Resolves if exists.
        /// </summary>
        /// <param name="resolver">The resolver.</param>
        /// <param name="type">The type.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static bool ResolveIfExists([NotNull] this IResolver resolver, [NotNull] Type type, [CanBeNull] out object instance)
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
