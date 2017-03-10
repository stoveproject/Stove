using Autofac.Extras.IocManager;

using Shouldly;

using Stove.Configuration;
using Stove.RabbitMQ.RabbitMQ;
using Stove.TestBase;

using Xunit;

namespace Stove.RabbitMQ.Tests
{
    public class StoveRabbitMQConfiguration_Tests : TestBaseWithLocalIocResolver
    {
        public StoveRabbitMQConfiguration_Tests()
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
            The<IModuleConfigurations>().StoveRabbitMQ().ShouldNotBeNull();
        }

        [Fact]
        public void configuration_settings_should_work()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            IStoveRabbitMQConfiguration configuration = The<IModuleConfigurations>().StoveRabbitMQ();
            configuration.HostAddress = "127.0.0.1";
            configuration.MaxRetryCount = 1;
            configuration.Password = "123456";
            configuration.QueueName = "Default";
            configuration.UseRetryMechanism = true;
            configuration.Username = "user";

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var configurationFromIoc = The<IStoveRabbitMQConfiguration>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            configurationFromIoc.HostAddress.ShouldBe("127.0.0.1");
            configurationFromIoc.MaxRetryCount.ShouldBe(configuration.MaxRetryCount);
            configurationFromIoc.Password.ShouldBe(configuration.Password);
            configurationFromIoc.QueueName.ShouldBe(configuration.QueueName);
            configurationFromIoc.UseRetryMechanism.ShouldBe(configuration.UseRetryMechanism);
        }
    }
}
