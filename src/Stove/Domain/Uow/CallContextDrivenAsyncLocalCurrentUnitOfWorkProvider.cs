using System.Collections.Concurrent;

using Autofac.Extras.IocManager;

using Stove.Log;
using Stove.Threading;

namespace Stove.Domain.Uow
{
    public class CallContextDrivenAsyncLocalCurrentUnitOfWorkProvider : ICurrentUnitOfWorkProvider, ITransientDependency
    {
        private const string ContextKey = "Stove.UnitOfWork.Current";
        private static readonly ConcurrentDictionary<string, IUnitOfWork> unitOfWorkDictionary = new ConcurrentDictionary<string, IUnitOfWork>();

        public CallContextDrivenAsyncLocalCurrentUnitOfWorkProvider()
        {
            Logger = NullLogger.Instance;
        }

        public ILogger Logger { get; set; }

        [DoNotInject]
        public IUnitOfWork Current
        {
            get => GetCurrentUow(Logger);
            set => SetCurrentUow(value, Logger);
        }

        private static IUnitOfWork GetCurrentUow(ILogger logger)
        {
            if (!(CallContext.GetData(ContextKey) is string unitOfWorkKey))
            {
                return null;
            }

            if (!unitOfWorkDictionary.TryGetValue(unitOfWorkKey, out IUnitOfWork unitOfWork))
            {
                CallContext.SetData(ContextKey, null);
                return null;
            }

            if (unitOfWork.IsDisposed)
            {
                logger.Warn("There is a unitOfWorkKey in CallContext but the UOW was disposed!");
                unitOfWorkDictionary.TryRemove(unitOfWorkKey, out unitOfWork);
                CallContext.SetData(ContextKey, null);
                return null;
            }

            return unitOfWork;
        }

        private static void SetCurrentUow(IUnitOfWork value, ILogger logger)
        {
            if (value == null)
            {
                ExitFromCurrentUowScope(logger);
                return;
            }

            if (CallContext.GetData(ContextKey) is string unitOfWorkKey)
            {
                if (unitOfWorkDictionary.TryGetValue(unitOfWorkKey, out IUnitOfWork outer))
                {
                    if (outer == value)
                    {
                        logger.Warn("Setting the same UOW to the CallContext, no need to set again!");
                        return;
                    }

                    value.Outer = outer;
                }
            }

            unitOfWorkKey = value.Id;
            if (!unitOfWorkDictionary.TryAdd(unitOfWorkKey, value))
            {
                throw new StoveException("Can not set unit of work! UnitOfWorkDictionary.TryAdd returns false!");
            }

            CallContext.SetData(ContextKey, unitOfWorkKey);
        }

        private static void ExitFromCurrentUowScope(ILogger logger)
        {
            if (!(CallContext.GetData(ContextKey) is string unitOfWorkKey))
            {
                logger.Warn("There is no current UOW to exit!");
                return;
            }

            if (!unitOfWorkDictionary.TryGetValue(unitOfWorkKey, out IUnitOfWork unitOfWork))
            {
                CallContext.SetData(ContextKey, null);
                return;
            }

            unitOfWorkDictionary.TryRemove(unitOfWorkKey, out unitOfWork);
            if (unitOfWork.Outer == null)
            {
                CallContext.SetData(ContextKey, null);
                return;
            }

            //Restore outer UOW
            string outerUnitOfWorkKey = unitOfWork.Outer.Id;
            if (!unitOfWorkDictionary.TryGetValue(outerUnitOfWorkKey, out unitOfWork))
            {
                //No outer UOW
                logger.Warn("Outer UOW key could not found in UnitOfWorkDictionary!");
                CallContext.SetData(ContextKey, null);
                return;
            }

            CallContext.SetData(ContextKey, outerUnitOfWorkKey);
        }
    }
}
