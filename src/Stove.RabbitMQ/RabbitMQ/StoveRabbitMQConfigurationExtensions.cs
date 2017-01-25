using Stove.Configuration;

namespace Stove.RabbitMQ.RabbitMQ
{
    public static class StoveRabbitMQConfigurationExtensions
    {
        /// <summary>
        ///     Gets RabbitMQ Configuration from Ioc Container.
        /// </summary>
        /// <param name="configurations">The configurations.</param>
        /// <returns></returns>
        public static IStoveRabbitMQConfiguration StoveRabbitMQ(this IModuleConfigurations configurations)
        {
            return configurations.StoveConfiguration.Get<IStoveRabbitMQConfiguration>();
        }
    }
}
