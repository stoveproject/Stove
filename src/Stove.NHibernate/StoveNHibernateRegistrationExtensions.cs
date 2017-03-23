using System;
using System.Collections.Generic;
using System.Reflection;

using Autofac.Extras.IocManager;

using Stove.Configuration;
using Stove.Domain.Repositories;
using Stove.NHibernate;
using Stove.NHibernate.Interceptors;
using Stove.NHibernate.Repositories;
using Stove.Orm;

namespace Stove
{
    public static class StoveNHibernateRegistrationExtensions
    {
        private const string OrmRegistrarContextKey = "OrmRegistrars";

        public static IIocBuilder UseStoveNHibernate(this IIocBuilder builder, Func<IStoveNHibernateConfiguration, IStoveNHibernateConfiguration> stoveNhConfigurer)
        {
            return builder.RegisterServices(r =>
            {
                r.Register(ctx => stoveNhConfigurer);
                r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
                r.RegisterGeneric(typeof(IRepository<>), typeof(NhRepositoryBase<>));
                r.RegisterGeneric(typeof(IRepository<,>), typeof(NhRepositoryBase<,>));
                r.Register(ctx =>
                {
                    var configuration = ctx.Resolver.Resolve<IStoveNHibernateConfiguration>();
                    var configurer = ctx.Resolver.Resolve<IStoveStartupConfiguration>().GetConfigurerIfExists<IStoveNHibernateConfiguration>();
                    configuration = configurer(configuration);

                    return configuration
                        .FluentConfiguration
                        .ExposeConfiguration(cfg => cfg.SetInterceptor(ctx.Resolver.Resolve<StoveNHibernateInterceptor>()))
                        .BuildSessionFactory();
                }, Lifetime.Singleton);

                var ormRegistrars = new List<IAdditionalOrmRegistrar>();
                ormRegistrars.Add(new NhBasedAdditionalOrmRegistrar(builder));
                r.UseBuilder(cb => { cb.Properties[OrmRegistrarContextKey] = ormRegistrars; });
            });
        }
    }
}
