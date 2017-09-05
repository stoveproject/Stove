using System;
using System.Reflection;

namespace Stove.Reflection.Extensions
{
	public static class TypeExtensions
	{
		public static Assembly GetAssembly(this Type type)
		{
			return type.GetTypeInfo().Assembly;
		}
	}
}
