using System;
using System.Collections.Generic;
using System.Reflection;

using Autofac.Extras.IocManager;

using Stove.Domain.Entities;
using Stove.NHibernate.Configuration;
using Stove.NHibernate.Enrichments;
using Stove.Orm;

namespace Stove.NHibernate
{
    public static class StoveNHibernateRegistrationExtensions
    {
        public static IIocBuilder UseStoveNHibernate(
            this IIocBuilder builder,
            Func<IStoveNHibernateConfiguration, IStoveNHibernateConfiguration> stoveNhConfigurer)
        {
            return builder.RegisterServices(r =>
            {
                var ormRegistrars = new List<ISecondaryOrmRegistrar>();
                r.OnRegistering += (sender, args) =>
                {
                    if (typeof(StoveSessionContext).IsAssignableFrom(args.ImplementationType))
                    {
                        NhRepositoryRegistrar.RegisterRepositories(args.ImplementationType, builder);
                        ormRegistrars.Add(new NhBasedSecondaryOrmRegistrar(builder, args.ImplementationType, SessionContextHelper.GetEntityTypeInfos, EntityHelper.GetPrimaryKeyType));
                        args.ContainerBuilder.Properties[StoveConsts.OrmRegistrarContextKey] = ormRegistrars;
                    }
                };
                r.Register(ctx => stoveNhConfigurer);
                r.RegisterGeneric(typeof(ISessionContextProvider<>),typeof(UnitOfWorkSessionContextProvider<>));
                r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            });
        }
    }
}
