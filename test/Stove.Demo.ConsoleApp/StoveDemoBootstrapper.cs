using System;
using System.Data.Entity.Infrastructure.Interception;

using Stove.Bootstrapping;
using Stove.Demo.ConsoleApp.DbContexes;
using Stove.EntityFramework;
using Stove.EntityFramework.EntityFramework.Interceptors;
using Stove.Hangfire;
using Stove.Mapster;
using Stove.RabbitMQ;
using Stove.Redis;

namespace Stove.Demo.ConsoleApp
{
    [DependsOn(
        typeof(StoveEntityFrameworkBootstrapper),
        typeof(StoveHangFireBootstrapper),
        typeof(StoveMapsterBootstrapper),
        typeof(StoveRabbitMQBootstrapper),
        typeof(StoveRedisBootstrapper)
    )]
    public class StoveDemoBootstrapper : StoveBootstrapper
    {
        public override void PreStart()
        {
            Configuration.Caching.Configure(DemoCacheName.Demo, cache => { cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(1); });
        }

        public override void Start()
        {
            Configuration.TypedConnectionStrings.Add(typeof(AnimalStoveDbContext), "Default");
            Configuration.TypedConnectionStrings.Add(typeof(PersonStoveDbContext), "Default");

            DbInterception.Add(Configuration.Resolver.Resolve<WithNoLockInterceptor>());
        }
    }
}
