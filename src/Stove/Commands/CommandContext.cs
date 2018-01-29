namespace Stove.Commands
{
    public class CommandContext
    {
        public string CorrelationId { get; set; }

        public string UserId { get; set; }

        public string BrowserInfo { get; set; }

        public string IpAddress { get; set; }

        public string HostName { get; set; }

        public int Duration { get; set; }
    }
}
