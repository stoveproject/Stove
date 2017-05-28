using Stove.MQ;
using Stove.RabbitMQ.Tests.Contracts;

using Xunit;

namespace Stove.RabbitMQ.Tests
{
    public class RabbitMQApplication_Tests : RabbitMQApplicationTestBase
    {
        public RabbitMQApplication_Tests()
        {
            Building(builder => { }).Ok();
        }

        //[Fact]
        public void event_should_be_send_to_consumer()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            var orderPlacedEvent = new OrderPlacedEvent { OrderId = 1 };

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            The<IMessageBus>().Publish(orderPlacedEvent);

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
        }
    }
}
