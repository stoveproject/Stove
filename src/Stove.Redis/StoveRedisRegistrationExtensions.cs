using System;
using System.Reflection;

using Autofac.Extras.IocManager;

using JetBrains.Annotations;

using Stove.Redis.Redis;
using Stove.Runtime.Caching;

namespace Stove.Redis
{
    public static class StoveRedisRegistrationExtensions
    {
        [NotNull]
        public static IIocBuilder UseStoveRedisCaching([NotNull] this IIocBuilder builder, [CanBeNull] Func<IStoveRedisCacheConfiguration, IStoveRedisCacheConfiguration> redisCacheConfigurer = null)
        {
            builder.RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
            builder.RegisterServices(r => r.Register<IStoveRedisCacheConfiguration, StoveRedisCacheConfiguration>(Lifetime.Singleton));
            builder.RegisterServices(r => r.RegisterType<StoveRedisCache>());
            builder.RegisterServices(r => r.Register<ICacheManager, StoveRedisCacheManager>());

            if (redisCacheConfigurer != null)
            {
                builder.RegisterServices(r => r.Register(ctx => redisCacheConfigurer));
            }

            return builder;
        }

        [NotNull]
        public static IIocBuilder UseTypelessRedisCacheSerializer([NotNull] this IIocBuilder builder)
        {
            builder.RegisterServices(r => { r.Register<IRedisCacheSerializer, TypelessRedisCacheSerializer>(); });
            return builder;
        }
    }
}
