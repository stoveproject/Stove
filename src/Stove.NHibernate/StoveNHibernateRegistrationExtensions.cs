using System.Reflection;

using Autofac.Extras.IocManager;

using Stove.Configuration;
using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.NHibernate.Interceptors;
using Stove.NHibernate.Repositories;
using Stove.NHibernate.Uow;

namespace Stove
{
    public static class StoveNHibernateRegistrationExtensions
    {
        public static IIocBuilder UseStoveNHibernate(this IIocBuilder builder)
        {
            return builder.RegisterServices(r =>
            {
                r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
                r.Register<IUnitOfWorkFilterExecuter, NhUnitOfWorkFilterExecuter>();
                r.RegisterType<StoveNHibernateInterceptor>();
                r.RegisterGeneric(typeof(IRepository<>), typeof(NhRepositoryBase<>));
                r.RegisterGeneric(typeof(IRepository<,>), typeof(NhRepositoryBase<,>));
                r.Register(ctx =>
                {
                    return ctx.Resolver
                              .Resolve<IStoveNHibernateConfiguration>()
                              .FluentConfiguration
                              .ExposeConfiguration(cfg => cfg.SetInterceptor(ctx.Resolver.Resolve<StoveNHibernateInterceptor>()))
                              .BuildSessionFactory();
                }, Lifetime.Singleton);
            });
        }
    }
}
