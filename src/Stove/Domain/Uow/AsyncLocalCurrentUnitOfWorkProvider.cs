using System.Threading;

using Autofac.Extras.IocManager;

using Stove.Log;

namespace Stove.Domain.Uow
{
    public class AsyncLocalCurrentUnitOfWorkProvider : ICurrentUnitOfWorkProvider
    {
        private static readonly AsyncLocal<LocalUowWrapper> asyncLocalUow = new AsyncLocal<LocalUowWrapper>();

        public AsyncLocalCurrentUnitOfWorkProvider()
        {
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        /// <inheritdoc />
        [DoNotInject]
        public IUnitOfWork Current
        {
            get => GetCurrentUow();
            set => SetCurrentUow(value);
        }

        private static IUnitOfWork GetCurrentUow()
        {
            IUnitOfWork uow = asyncLocalUow.Value?.UnitOfWork;
            if (uow == null)
            {
                return null;
            }

            if (uow.IsDisposed)
            {
                asyncLocalUow.Value = null;
                return null;
            }

            return uow;
        }

        private static void SetCurrentUow(IUnitOfWork value)
        {
            lock (asyncLocalUow)
            {
                if (value == null)
                {
                    if (asyncLocalUow.Value == null)
                    {
                        return;
                    }

                    if (asyncLocalUow.Value.UnitOfWork?.Outer == null)
                    {
                        asyncLocalUow.Value.UnitOfWork = null;
                        asyncLocalUow.Value = null;
                        return;
                    }

                    asyncLocalUow.Value.UnitOfWork = asyncLocalUow.Value.UnitOfWork.Outer;
                }
                else
                {
                    if (asyncLocalUow.Value?.UnitOfWork == null)
                    {
                        if (asyncLocalUow.Value != null)
                        {
                            asyncLocalUow.Value.UnitOfWork = value;
                        }

                        asyncLocalUow.Value = new LocalUowWrapper(value);
                        return;
                    }

                    value.Outer = asyncLocalUow.Value.UnitOfWork;
                    asyncLocalUow.Value.UnitOfWork = value;
                }
            }
        }

        private class LocalUowWrapper
        {
            public LocalUowWrapper(IUnitOfWork unitOfWork)
            {
                UnitOfWork = unitOfWork;
            }

            public IUnitOfWork UnitOfWork { get; set; }
        }
    }
}
