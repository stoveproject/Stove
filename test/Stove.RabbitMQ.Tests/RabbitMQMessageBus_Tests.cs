using MassTransit;
using MassTransit.Testing;

using NSubstitute;

using Stove.MQ;
using Stove.TestBase;

using Xunit;

namespace Stove.RabbitMQ.Tests
{
    public class RabbitMQMessageBus_Tests : TestBaseWithLocalIocResolver
    {
        private readonly IBus _bus;
        private readonly IStoveRabbitMQConfiguration _configuration;

        public RabbitMQMessageBus_Tests()
        {
            _bus = Substitute.For<IBus>();
            _configuration = Substitute.For<IStoveRabbitMQConfiguration>();

            Building(builder =>
                {
                    builder.RegisterServices(r =>
                    {
                        r.Register<IMessageBus, StoveRabbitMQMessageBus>();
                        r.Register(ctx => _bus);
                        r.Register(ctx=>_configuration);
                    });
                })
                .Ok();
        }

        [Fact]
        public void publish_object_should_work()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var messageBus = The<IMessageBus>();
            var message = new { Message = "Hi" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            messageBus.Publish(message);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            _bus.Received().Publish(message);
        }

        [Fact]
        public void publish_with_object_should_work()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var messageBus = The<IMessageBus>();
            var message = new { Message = "Hi" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            messageBus.Publish((object)message);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            _bus.Received().Publish((object)message);
        }

        [Fact]
        public void publish_with_TMessage_should_work()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var messageBus = The<IMessageBus>();
            var message = new RabbitMqTestMessage { Message = "Hi" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            messageBus.Publish(message);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            _bus.Received().Publish<IRabbitMqTestMessage>(message);
        }

        [Fact]
        public void publish_with_TMessage_as_object_should_work()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var messageBus = The<IMessageBus>();
            var message = new { Message = "Hi" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            messageBus.Publish<IRabbitMqTestMessage>(message);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            _bus.Received().Publish<IRabbitMqTestMessage>(message);
        }

        [Fact]
        public void publish_object_with_type_should_work()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var messageBus = The<IMessageBus>();
            var message = new RabbitMqTestMessage { Message = "Hi" };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            messageBus.Publish(message, typeof(IRabbitMqTestMessage));

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            _bus.Received().Publish(message, typeof(IRabbitMqTestMessage));
        }

        private interface IRabbitMqTestMessage
        {
            string Message { get; set; }
        }

        private class RabbitMqTestMessage : IRabbitMqTestMessage
        {
            public string Message { get; set; }
        }
    }
}
