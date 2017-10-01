using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Autofac;
using Autofac.Core;

using Stove.Log;

using Module = Autofac.Module;

namespace Stove.Serilog
{
	public class SerilogRegistrarModule : Module
	{
		protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
		{
			registration.Preparing += OnComponentPreparing;

			registration.Activated += (sender, e) => InjectLoggerProperties(e);
		}

		private void InjectLoggerProperties(ActivatedEventArgs<object> e)
		{
			Type instanceType = e.Instance.GetType();

			// Get all the injectable properties to set.
			// If you wanted to ensure the properties were only UNSET properties,
			// here's where you'd do it.
			IEnumerable<PropertyInfo> properties = instanceType
				.GetProperties(BindingFlags.Public | BindingFlags.Instance)
				.Where(p => p.PropertyType == typeof(ILogger) && p.CanWrite && p.GetIndexParameters().Length == 0);

			// Set the properties located.
			foreach (PropertyInfo propToSet in properties)
			{
				propToSet.SetValue(e.Instance, new StoveSerilogLogger(e.Context.Resolve<global::Serilog.ILogger>().ForContext(instanceType)), null);
			}
		}

		private void OnComponentPreparing(object sender, PreparingEventArgs e)
		{
			Type t = e.Component.Activator.LimitType;
			e.Parameters = e.Parameters.Union(
				new[]
				{
					new ResolvedParameter((p, ctx) => p.ParameterType == typeof(ILogger),
						(p, ctx) => new StoveSerilogLogger(ctx.Resolve<global::Serilog.ILogger>().ForContext(t)))
				});
		}
	}
}
