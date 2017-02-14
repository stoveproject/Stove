using Autofac;
using Autofac.Extras.IocManager;

using Shouldly;

using Stove.Configuration;
using Stove.RabbitMQ.RabbitMQ;
using Stove.TestBase;

using Xunit;

namespace Stove.RabbitMQ.Tests
{
    public class StoveRabbitMQConfigurationExtensions_Tests : TestBaseWithLocalIocResolver
    {
        public StoveRabbitMQConfigurationExtensions_Tests()
        {
            Building(builder =>
            {
                builder.RegisterServices(r =>
                {
                    r.Register<IModuleConfigurations, ModuleConfigurations>(Lifetime.Singleton);
                    r.Register<IStoveStartupConfiguration, StoveStartupConfiguration>(Lifetime.Singleton);
                    r.Register<IStoveRabbitMQConfiguration, StoveRabbitMQConfiguration>(Lifetime.Singleton);
                });
            }).Ok();
        }

        [Fact]
        public void extension_should_be_instantiatable()
        {
            LocalResolver.Container.Resolve<IModuleConfigurations>().StoveRabbitMQ().ShouldNotBeNull();
        }
    }
}
