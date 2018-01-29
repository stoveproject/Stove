namespace Stove.Commands
{
    public abstract class SequencedCommand : Command
    {
        public readonly int Position;
        public readonly int Size;

        protected SequencedCommand(int position, int size)
        {
            Position = position;
            Size = size;
        }

        public bool IsLast()
        {
            return Position == Size;
        }
    }
}
