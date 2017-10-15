using System;

using Autofac.Extras.IocManager;

using Couchbase;
using Couchbase.Core;

using Stove.Couchbase.Couchbase.Configuration;
using Stove.Couchbase.Couchbase.Repositories;
using Stove.Domain.Repositories;
using Stove.Reflection.Extensions;

namespace Stove.Couchbase.Couchbase
{
    public static class StoveCouchbaseRegistrationExtensions
    {
        public static IIocBuilder UseStoveCouchbase(this IIocBuilder builder, Func<IStoveCouchbaseConfiguration, IStoveCouchbaseConfiguration> configurer)
        {
            return builder
                .RegisterServices(r =>
                {
                    r.RegisterGeneric(typeof(IRepository<,>), typeof(CouchbaseRepositoryBase<>));
                    r.Register(ctx => configurer);
                    r.Register<ICluster>(ctx =>
                    {
                        var cfg = ctx.Resolver.Resolve<IStoveCouchbaseConfiguration>();
                        return new Cluster(cfg.ClientConfiguration);
                    }, Lifetime.Singleton);

                    r.RegisterAssemblyByConvention(typeof(StoveCouchbaseRegistrationExtensions).GetAssembly());
                });
        }
    }
}
