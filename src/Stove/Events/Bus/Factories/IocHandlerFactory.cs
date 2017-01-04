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
        private IScopeResolver _childScope;

        /// <summary>
        ///     Creates a new instance of <see cref="IocHandlerFactory" /> class.
        /// </summary>
        /// <param name="scopeResolver">The scope resolver.</param>
        /// <param name="handlerType">Type of the handler</param>
        public IocHandlerFactory(IScopeResolver scopeResolver, Type handlerType)
        {
            HandlerType = handlerType;
            _scopeResolver = scopeResolver;
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
            _childScope = _scopeResolver.BeginScope();
            return (IEventHandler)_childScope.Resolve(HandlerType);
        }

        /// <summary>
        ///     Releases the handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public void ReleaseHandler(IEventHandler handler)
        {
            _childScope.Dispose();
        }
    }
}
