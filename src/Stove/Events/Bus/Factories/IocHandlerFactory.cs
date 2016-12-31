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
        private readonly IScopeResolver _scopeResolver;

        /// <summary>
        ///     Creates a new instance of <see cref="IocHandlerFactory" /> class.
        /// </summary>
        /// <param name="scopeResolver"></param>
        /// <param name="handlerType">Type of the handler</param>
        public IocHandlerFactory(IScopeResolver scopeResolver, Type handlerType)
        {
            _scopeResolver = scopeResolver;
            HandlerType = handlerType;
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
            return (IEventHandler)_scopeResolver.Resolve(HandlerType);
        }

        /// <summary>
        ///     Releases handler object using Ioc container.
        /// </summary>
        /// <param name="handler">Handler to be released</param>
        public void ReleaseHandler(IEventHandler handler)
        {
            _scopeResolver.Dispose();
        }
    }
}
