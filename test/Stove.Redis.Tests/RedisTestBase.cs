using System.Reflection;

using NSubstitute;

using StackExchange.Redis;
using StackExchange.Redis.Extensions.Core.Configuration;

using Stove.TestBase;

namespace Stove.Redis.Tests
{
    public abstract class RedisTestBase : ApplicationTestBase<StoveRedisTestBootstrapper>
    {
        protected RedisTestBase()
        {
            Building(builder =>
            {
                builder.UseStoveRedisCaching(configuration =>
                       {
                           configuration.CachingConfiguration = Substitute.For<IRedisCachingConfiguration>();
                           configuration.ConfigurationOptions = new ConfigurationOptions();
                           return configuration;
                       })
                       .RegisterServices(r => r.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly()));
            });
        }
    }
}
