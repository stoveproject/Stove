using System;
using System.Threading;

using Autofac;
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
        private static readonly ReaderWriterLockSlim _rw = new ReaderWriterLockSlim();
        private readonly IResolver _resolver;
        private ILifetimeScope _childScope;

        /// <summary>
        ///     Creates a new instance of <see cref="IocHandlerFactory" /> class.
        /// </summary>
        /// <param name="scopeResolver">The scope resolver.</param>
        /// <param name="handlerType">Type of the handler</param>
        public IocHandlerFactory(IResolver scopeResolver, Type handlerType)
        {
            HandlerType = handlerType;
            _resolver = scopeResolver;
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
            _rw.EnterUpgradeableReadLock();

            _rw.EnterWriteLock();

            _childScope = _resolver.Resolve<ILifetimeScope>().BeginLifetimeScope();

            var handler = (IEventHandler)_childScope.Resolve(HandlerType);

            _rw.ExitWriteLock();

            return handler;
        }

        /// <summary>
        ///     Releases the handler.
        /// </summary>
        /// <param name="handler">The handler.</param>
        public void ReleaseHandler(IEventHandler handler)
        {
           _rw.EnterWriteLock();

            _childScope.Dispose();

            _childScope = null;

            _rw.ExitWriteLock();

            _rw.ExitUpgradeableReadLock();
        }
    }
}
