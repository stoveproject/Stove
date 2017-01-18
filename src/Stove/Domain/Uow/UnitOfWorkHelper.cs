using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Autofac.Core;

using Stove.Application.Services;
using Stove.Domain.Repositories;
using Stove.Extensions;

namespace Stove.Domain.Uow
{
    /// <summary>
    ///     A helper class to simplify unit of work process.
    /// </summary>
    internal static class UnitOfWorkHelper
    {
        /// <summary>
        ///     Returns true if UOW must be used for given type as convention.
        /// </summary>
        /// <param name="type">Type to check</param>
        public static bool IsConventionalUowClass(Type type)
        {
            return typeof(IRepository).IsAssignableFrom(type) || typeof(IApplicationService).IsAssignableFrom(type) || typeof(IDapperRepository).IsAssignableFrom(type);
        }

        /// <summary>
        ///     Returns true if given method has UnitOfWorkAttribute attribute.
        /// </summary>
        /// <param name="methodInfo">Method info to check</param>
        public static bool HasUnitOfWorkAttribute(MemberInfo methodInfo)
        {
            return methodInfo.IsDefined(typeof(UnitOfWorkAttribute), true);
        }

        /// <summary>
        ///     Determines whether <see cref="UnitOfWorkAttribute" />  [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///     <c>true</c> if <see cref="UnitOfWorkAttribute" /> [the specified type]; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasUnitOfWorkAttribute(Type type)
        {
            return type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Any(HasUnitOfWorkAttribute);
        }

        /// <summary>
        ///     Returns UnitOfWorkAttribute it exists.
        /// </summary>
        /// <param name="methodInfo">Method info to check</param>
        public static UnitOfWorkAttribute GetUnitOfWorkAttributeOrNull(MemberInfo methodInfo)
        {
            object[] attrs = methodInfo.GetCustomAttributes(typeof(UnitOfWorkAttribute), false);
            if (attrs.Length <= 0)
            {
                return null;
            }

            return (UnitOfWorkAttribute)attrs[0];
        }

        internal static List<TypedService> GetOrDefaultConventionalUowClassesAsTypedService(this IEnumerable<Service> services)
        {
            return services.Where(x => IsConventionalUowClass(x.As<TypedService>().ServiceType))
                           .Select(x => x.As<TypedService>())
                           .ToList();
        }
    }
}
