using JetBrains.Annotations;

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
        [NotNull]
        public static IStoveRabbitMQConfiguration StoveRabbitMQ([NotNull] this IModuleConfigurations configurations)
        {
            return configurations.StoveConfiguration.Get<IStoveRabbitMQConfiguration>();
        }
    }
}
