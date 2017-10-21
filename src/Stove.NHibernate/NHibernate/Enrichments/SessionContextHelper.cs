using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Stove.Domain.Entities;
using Stove.Reflection;

namespace Stove.NHibernate.Enrichments
{
    public static class SessionContextHelper
    {
        public static IEnumerable<EntityTypeInfo> GetEntityTypeInfos(Type stoveContextType)
        {
            return
                from property in stoveContextType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where
                    ReflectionHelper.IsAssignableToGenericType(property.PropertyType, typeof(IStoveSessionSet<>)) &&
                    ReflectionHelper.IsAssignableToGenericType(property.PropertyType.GenericTypeArguments[0], typeof(IEntity<>))
                select new EntityTypeInfo(property.PropertyType.GenericTypeArguments[0], property.DeclaringType);
        }
    }
}
