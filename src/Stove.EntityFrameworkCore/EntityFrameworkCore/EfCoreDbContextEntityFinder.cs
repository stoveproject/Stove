using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.EntityFrameworkCore;

using Stove.Domain.Entities;
using Stove.Reflection;

namespace Stove.EntityFrameworkCore
{
	internal static class EfCoreDbContextEntityFinder
	{
		public static IEnumerable<EntityTypeInfo> GetEntityTypeInfos(Type dbContextType)
		{
			return
				from property in dbContextType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				where
				ReflectionHelper.IsAssignableToGenericType(property.PropertyType, typeof(DbSet<>)) &&
				ReflectionHelper.IsAssignableToGenericType(property.PropertyType.GenericTypeArguments[0], typeof(IEntity<>))
				select new EntityTypeInfo(property.PropertyType.GenericTypeArguments[0], property.DeclaringType);
		}
	}
}
