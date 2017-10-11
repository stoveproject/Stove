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
    internal class SingleInstanceHandlerFactory : IEventHandlerFactory
    {
        /// <summary>
        /// </summary>
        /// <param name="handler"></param>
        public SingleInstanceHandlerFactory(IEventHandler handler)
        {
            HandlerInstance = handler;
        }

        /// <summary>
        ///     The event handler instance.
        /// </summary>
        public IEventHandler HandlerInstance { get; }

        public IEventHandler GetHandler()
        {
            return HandlerInstance;
        }

        public void ReleaseHandler(IEventHandler handler)
        {
        }
    }
}
