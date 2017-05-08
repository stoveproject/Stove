using Stove.Configuration;

namespace Stove.RabbitMQ
{
    public interface IStoveRabbitMQConfiguration
    {
        string HostAddress { get; set; }

        string Username { get; set; }

        string Password { get; set; }

        string QueueName { get; set; }

        bool UseRetryMechanism { get; set; }

        int MaxRetryCount { get; set; }

        IStoveStartupConfiguration Configuration { get; }
    }
}
