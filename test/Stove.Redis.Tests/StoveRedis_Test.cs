using NSubstitute;

using Stove.Runtime.Caching;

using Xunit;

namespace Stove.Redis.Tests
{
    public class StoveRedis_Test : RedisTestBase
    {
        public StoveRedis_Test()
        {
            Building(builder =>
            {
                var fakeCacheClientProvider = Substitute.For<IStoveRedisCacheClientProvider>();
                builder.RegisterServices(r => r.Register(ctx => fakeCacheClientProvider));

            }).Ok();
        }

        [Fact]
        public void should_work()
        {
            The<ICacheManager>().GetCache("RedisTest").Get("apple", s => new { Name = "apple" });

            The<IStoveRedisCacheClientProvider>().GetClient().Received();
        }
    }
}
