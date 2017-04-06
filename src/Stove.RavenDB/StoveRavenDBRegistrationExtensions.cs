using System;
using System.Reflection;

using Autofac;
using Autofac.Extras.IocManager;

using Raven.Client;
using Raven.Client.Document;

using Stove.Domain.Repositories;
using Stove.RavenDB.Configuration;
using Stove.RavenDB.Repositories;

namespace Stove
{
    public static class StoveRavenDBRegistrationExtensions
    {
        public static IIocBuilder UseStoveRavenDB(this IIocBuilder builder, Func<IStoveRavenDBConfiguration, IStoveRavenDBConfiguration> configurer)
        {
            return builder.RegisterServices(r =>
            {
                r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
                r.UseBuilder(cb =>
                {
                    cb.RegisterGeneric(typeof(RavenDBRepositoryBase<>)).As(typeof(IRepository<>)).PropertiesAutowired(new DoNotInjectAttributePropertySelector());
                    cb.RegisterGeneric(typeof(RavenDBRepositoryBase<,>)).As(typeof(IRepository<,>)).PropertiesAutowired(new DoNotInjectAttributePropertySelector());
                });
                r.Register(ctx => configurer);
                r.Register<IDocumentStore>(ctx =>
                {
                    var cfg = ctx.Resolver.Resolve<IStoveRavenDBConfiguration>();
                    return new DocumentStore
                    {
                        Url = cfg.Url,
                        DefaultDatabase = cfg.DefaultDatabase
                    };
                }, Lifetime.Singleton);
            });
        }
    }
}
