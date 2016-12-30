using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Autofac.Builder;
using Autofac.Core;
using Autofac.Extras.DynamicProxy;
using Autofac.Extras.IocManager;

using Castle.DynamicProxy;

using Stove.Domain.Uow;
using Stove.Events.Bus;

namespace Stove
{
    public static class StoveRegistrationExtensions
    {
        public static IIocBuilder UseStove(this IIocBuilder builder)
        {
            builder.RegisterServices(r => r.UseBuilder(containerBuilder => containerBuilder.RegisterCallback(registry => registry.Registered += RegistryOnRegistered)));
            builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
            builder.RegisterServices(r => r.Register<IGuidGenerator>(context => SequentialGuidGenerator.Instance));
            builder.RegisterServices(r => r.Register<IEventBus>(context => EventBus.Default));
            return builder;
        }

        private static void RegistryOnRegistered(object sender, ComponentRegisteredEventArgs componentRegisteredEventArgs)
        {
            HandleUnitOfWorkRegistar(componentRegisteredEventArgs);
        }

        private static void HandleUnitOfWorkRegistar(ComponentRegisteredEventArgs args)
        {
            List<TypedService> uowClasses = args.ComponentRegistration.Services.GetOrDefaultConventionalUowClassesAsTypedService();

            if (!uowClasses.Any())
            {
                return;
            }

            Type implementationType = uowClasses.FirstOrDefault(x => !x.ServiceType.IsInterface)?.ServiceType;
            List<Type> interfaceTypes = uowClasses.Where(x => x.ServiceType != implementationType).Select(t => t.ServiceType).ToList();
            if (implementationType != null && interfaceTypes.Any())
            {
                args.ComponentRegistry.Register(
                    RegistrationBuilder.ForType(implementationType)
                                       .As(interfaceTypes.ToArray())
                                       .EnableClassInterceptors(ProxyGenerationOptions.Default, interfaceTypes.ToArray())
                                       .InterceptedBy(typeof(UnitOfWorkInterceptor))
                                       .InjectPropertiesAsAutowired()
                                       .InstancePerDependency()
                                       .CreateRegistration()
                );
            }
        }
    }
}
