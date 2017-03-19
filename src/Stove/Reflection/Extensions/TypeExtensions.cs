using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using FluentAssemblyScanner;

using Stove.IO;

namespace Stove.Reflection.Extensions
{
    public static class TypeExtensions
    {
        public static IList<Type> AssignedTypesInAssembly(this Type @this, Assembly assembly)
        {
            return AssemblyScanner.FromAssembly(assembly)
                                  .BasedOn(@this)
                                  .Filter()
                                  .Classes()
                                  .NonStatic()
                                  .Scan();
        }

        public static IList<Type> AssignedTypes(this Type @this)
        {
            return AssemblyScanner.FromAssemblyInDirectory(new AssemblyFilter(AppDomain.CurrentDomain.GetActualDomainPath()))
                                  .IncludeNonPublicTypes()
                                  .BasedOn(@this)
                                  .Filter()
                                  .Classes()
                                  .NonStatic()
                                  .Scan();
        }

        /// <summary>
        ///     Checks whether <paramref name="child" /> implements/inherits <paramref name="parent" />
        /// </summary>
        /// <param name="child">Current Type</param>
        /// <param name="parent">Parent Type</param>
        /// <returns></returns>
        public static bool IsInheritsOrImplements(this Type child, Type parent)
        {
            if (child == parent)
            {
                return false;
            }

            if (child.IsSubclassOf(parent))
            {
                return true;
            }

            Type[] parameters = parent.GetGenericArguments();

            bool isParameterLessGeneric = !(parameters.Length > 0 &&
                                            (parameters[0].Attributes & TypeAttributes.BeforeFieldInit) ==
                                            TypeAttributes.BeforeFieldInit);

            while (child != null && child != typeof(object))
            {
                Type cur = GetFullTypeDefinition(child);
                if (parent == cur ||
                    isParameterLessGeneric &&
                    cur.GetInterfaces().Select(GetFullTypeDefinition).Contains(GetFullTypeDefinition(parent)))
                {
                    return true;
                }

                if (!isParameterLessGeneric)
                {
                    if (GetFullTypeDefinition(parent) == cur && !cur.IsInterface)
                    {
                        if (VerifyGenericArguments(GetFullTypeDefinition(parent), cur))
                        {
                            if (VerifyGenericArguments(parent, child))
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        if (child.GetInterfaces()
                                 .Where(i => GetFullTypeDefinition(parent) == GetFullTypeDefinition(i))
                                 .Any(item => VerifyGenericArguments(parent, item)))
                        {
                            return true;
                        }
                    }
                }

                child = child.BaseType;
            }

            return false;
        }

        private static Type GetFullTypeDefinition(Type type)
        {
            return type.IsGenericType ? type.GetGenericTypeDefinition() : type;
        }

        private static bool VerifyGenericArguments(Type parent, Type child)
        {
            Type[] childArguments = child.GetGenericArguments();
            Type[] parentArguments = parent.GetGenericArguments();
            if (childArguments.Length == parentArguments.Length)
            {
                for (var i = 0; i < childArguments.Length; i++)
                {
                    if (childArguments[i].Assembly != parentArguments[i].Assembly ||
                        childArguments[i].Name != parentArguments[i].Name ||
                        childArguments[i].Namespace != parentArguments[i].Namespace)
                    {
                        if (!childArguments[i].IsSubclassOf(parentArguments[i]))
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
