using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Autofac.Core;

using NLog;

using ILogger = Stove.Log.ILogger;
using Module = Autofac.Module;

namespace Stove
{
	public class NLogRegistrarModule : Module
	{
		private static void InjectLoggerProperties(object instance)
		{
			Type instanceType = instance.GetType();

			// Get all the injectable properties to set.
			// If you wanted to ensure the properties were only UNSET properties,
			// here's where you'd do it.
			IEnumerable<PropertyInfo> properties = instanceType
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(p => p.PropertyType == typeof(ILogger) && p.CanWrite && p.GetIndexParameters().Length == 0);

			// Set the properties located.
			foreach (PropertyInfo propToSet in properties)
			{
				propToSet.SetValue(instance, new StoveNLogLogger(LogManager.GetLogger(instanceType.FullName)), null);
			}
		}

		private static void OnComponentPreparing(object sender, PreparingEventArgs e)
		{
			Type t = e.Component.Activator.LimitType;
			e.Parameters = e.Parameters.Union(
				new[]
				{
					new ResolvedParameter((p, i) => p.ParameterType == typeof(ILogger),
						(p, i) => new StoveNLogLogger(LogManager.GetLogger(t.FullName)))
				});
		}

		protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
		{
			registration.Preparing += OnComponentPreparing;

			registration.Activated += (sender, e) => InjectLoggerProperties(e.Instance);
		}
	}
}
