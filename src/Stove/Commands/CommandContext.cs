namespace Stove
{
    public class CommandContext
    {
        public readonly string CorrelationId;

        public CommandContext(string correlationId)
        {
            CorrelationId = correlationId;
        }
    }
}
