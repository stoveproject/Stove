using System;

using Stove.Bootstrapping;
using Stove.Demo.ConsoleApp.DbContexes;
using Stove.EntityFramework.Bootstrappers;
using Stove.Hangfire.Bootsrappers;
using Stove.Mapster.Bootstrappers;

namespace Stove.Demo.ConsoleApp
{
    [DependsOn(
        typeof(StoveEntityFrameworkBootstrapper),
        typeof(StoveHangFireBootstrapper),
        typeof(StoveMapsterBootstrapper)
    )]
    public class StoveDemoBootstrapper : StoveBootstrapper
    {
        public override void PreStart()
        {
            Configuration.Caching.Configure(DemoCacheName.Demo, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(1);
            });
        }

        public override void Start()
        {
            Configuration.TypedConnectionStrings.Add(typeof(AnimalStoveDbContext), "Default");
            Configuration.TypedConnectionStrings.Add(typeof(PersonStoveDbContext), "Default");
        }
    }
}
