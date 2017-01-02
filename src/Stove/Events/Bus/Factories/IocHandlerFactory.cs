using System;

using Autofac.Extras.IocManager;

using Stove.Events.Bus.Handlers;

namespace Stove.Events.Bus.Factories
{
    /// <summary>
    ///     This <see cref="IEventHandlerFactory" /> implementation is used to get/release
    ///     handlers using Ioc.
    /// </summary>
    public class IocHandlerFactory : IEventHandlerFactory
    {
        private readonly IResolver _resolver;

        /// <summary>
        ///     Creates a new instance of <see cref="IocHandlerFactory" /> class.
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="handlerType">Type of the handler</param>
        public IocHandlerFactory(IResolver resolver, Type handlerType)
        {
            HandlerType = handlerType;
            _resolver = resolver;
        }

        /// <summary>
        ///     Type of the handler.
        /// </summary>
        public Type HandlerType { get; }

        /// <summary>
        ///     Resolves handler object from Ioc container.
        /// </summary>
        /// <returns>Resolved handler object</returns>
        public IEventHandler GetHandler()
        {
            return (IEventHandler)_resolver.Resolve(HandlerType);
        }
    }
}
