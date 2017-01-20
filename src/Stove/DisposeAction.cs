using System;
using System.Threading;

using JetBrains.Annotations;

namespace Stove
{
    /// <summary>
    ///     https://referencesource.microsoft.com/#system.web/Util/DisposableAction.cs,e705cb82d7fc864f
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class DisposeAction : IDisposable
    {
        public static readonly DisposeAction Null = new DisposeAction(null);
        private Action _action;

        /// <summary>
        ///     Creates a new <see cref="DisposeAction" /> object.
        /// </summary>
        /// <param name="action">Action to be executed when this object is disposed.</param>
        public DisposeAction([CanBeNull] Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            // Interlocked allows the continuation to be executed only once
            Action action = Interlocked.Exchange(ref _action, null);
            action?.Invoke();
        }
    }
}
