using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using JetBrains.Annotations;

namespace Stove.Reflection.Extensions
{
    public static class MemberInfoExtensions
    {
        /// <summary>
        ///     Gets a single attribute for a member.
        /// </summary>
        /// <typeparam name="TAttribute">Type of the attribute</typeparam>
        /// <param name="memberInfo">The member that will be checked for the attribute</param>
        /// <param name="inherit">Include inherited attributes</param>
        /// <returns>Returns the attribute object if found. Returns null if not found.</returns>
        [CanBeNull]
        public static TAttribute GetSingleAttributeOrNull<TAttribute>(this MemberInfo memberInfo, bool inherit = true)
            where TAttribute : Attribute
        {
			if (memberInfo == null)
	        {
		        throw new ArgumentNullException(nameof(memberInfo));
	        }

	        List<object> attrs = memberInfo.GetCustomAttributes(typeof(TAttribute), inherit).ToList();
	        if (attrs.Any())
	        {
		        return (TAttribute)attrs[0];
	        }

	        return default;
		}

        [CanBeNull]
        public static TAttribute GetSingleAttributeOfTypeOrBaseTypesOrNull<TAttribute>(this Type type, bool inherit = true)
            where TAttribute : Attribute
        {
            var attr = type.GetTypeInfo().GetSingleAttributeOrNull<TAttribute>();
            if (attr != null)
            {
                return attr;
            }

            if (type.GetTypeInfo().BaseType == null)
            {
                return null;
            }

            return type.GetTypeInfo().BaseType.GetSingleAttributeOfTypeOrBaseTypesOrNull<TAttribute>(inherit);
        }
    }
}
