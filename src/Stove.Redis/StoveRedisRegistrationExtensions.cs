using System;
using System.Reflection;

using Autofac.Extras.IocManager;

using Stove.Redis.Redis;
using Stove.Runtime.Caching;

namespace Stove.Redis
{
    public static class StoveRedisRegistrationExtensions
    {
        public static IIocBuilder UseStoveRedisCaching(this IIocBuilder builder, Func<StoveRedisCacheOptions, StoveRedisCacheOptions> redisCacheConfigurer = null)
        {
            builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
            builder.RegisterServices(r => r.RegisterType<StoveRedisCache>());
            builder.RegisterServices(r => r.RegisterType<StoveRedisCacheOptions>(Lifetime.Singleton));
            builder.RegisterServices(r => r.Register<ICacheManager, StoveRedisCacheManager>());

            if (redisCacheConfigurer != null)
            {
                builder.RegisterServices(r => r.Register(ctx => redisCacheConfigurer));
            }

            return builder;
        }
    }
}
