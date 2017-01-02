using Stove.Events.Bus.Handlers;

namespace Stove.Events.Bus.Factories
{
    /// <summary>
    ///     Defines an interface for factories those are responsible to create/get and release of event handlers.
    /// </summary>
    public interface IEventHandlerFactory
    {
        /// <summary>
        ///     Gets an event handler.
        /// </summary>
        /// <returns>The event handler</returns>
        IEventHandler GetHandler();
    }
}
