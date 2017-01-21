namespace Stove.RabbitMQ.RabbitMQ
{
    public interface IStoveRabbitMQConfiguration
    {
        string HostAddress { get; set; }

        string Username { get; set; }

        string Password { get; set; }

        string QueueName { get; set; }
    }
}
