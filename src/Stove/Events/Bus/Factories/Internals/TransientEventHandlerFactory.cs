using System;

using Stove.Events.Bus.Handlers;

namespace Stove.Events.Bus.Factories.Internals
{
    /// <inheritdoc />
    /// <summary>
    ///     This <see cref="T:Stove.Events.Bus.Factories.IEventHandlerFactory" /> implementation is used to handle events
    ///     by a single instance object.
    /// </summary>
    /// <remarks>
    ///     This class always gets the same single instance of handler.
    /// </remarks>
    internal class TransientEventHandlerFactory<THandler> : IEventHandlerFactory
        where THandler : IEventHandler, new()
    {
        /// <inheritdoc />
        /// <summary>
        ///     Creates a new instance of the handler object.
        /// </summary>
        /// <returns>The handler object</returns>
        public IEventHandler GetHandler()
        {
            return new THandler();
        }

        /// <inheritdoc />
        /// <summary>
        ///     Disposes the handler object if it's <see cref="T:System.IDisposable" />. Does nothing if it's not.
        /// </summary>
        /// <param name="handler">Handler to be released</param>
        public void ReleaseHandler(IEventHandler handler)
        {
            (handler as IDisposable)?.Dispose();
        }
    }
}
