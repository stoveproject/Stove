using System;

namespace Stove
{
    public abstract class SequencedCommand : Command
    {
        public readonly int Position;
        public readonly Guid Sequence;
        public readonly int Size;

        protected SequencedCommand(Guid sequence, int position, int size)
        {
            Sequence = sequence;
            Position = position;
            Size = size;
        }

        public bool IsLast()
        {
            return Position == Size;
        }
    }
}
