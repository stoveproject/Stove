using System;
using System.Diagnostics.CodeAnalysis;

namespace Stove.Utils.Etc
{
    [ExcludeFromCodeCoverage]
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
