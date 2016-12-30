using System;

namespace Stove.Utils.Etc
{
    internal sealed class NullDisposable : IDisposable
    {
        private NullDisposable()
        {
        }

        public static NullDisposable Instance { get; } = new NullDisposable();

        public void Dispose()
        {
        }
    }
}
