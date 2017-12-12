using System;
using System.Diagnostics;
using System.Transactions;

using Stove.Domain.Uow;
using Stove.Log;
using Stove.MQ;
using Stove.ObjectMapping;

namespace Stove
{
    /// <summary>
    ///     This class can be used as a base class for services.
    ///     It has some useful objects property-injected and has some basic methods
    ///     most of services may need to.
    /// </summary>
    public abstract class StoveComponentBase
    {
        private IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        ///     Constructor.
        /// </summary>
        protected StoveComponentBase()
        {
            Logger = NullLogger.Instance;
            ObjectMapper = NullObjectMapper.Instance;
            MessageBus = NullMessageBus.Instance;
        }

        /// <summary>
        ///     Reference to <see cref="IUnitOfWorkManager" />.
        /// </summary>
        public IUnitOfWorkManager UnitOfWorkManager
        {
            get
            {
                if (_unitOfWorkManager == null)
                {
                    throw new StoveException("Must set UnitOfWorkManager before use it.");
                }

                return _unitOfWorkManager;
            }
            set => _unitOfWorkManager = value;
        }

        /// <summary>
        ///     Gets current unit of work.
        /// </summary>
        protected IActiveUnitOfWork CurrentUnitOfWork => UnitOfWorkManager.Current;

        /// <summary>
        ///     Reference to the logger to write logs.
        /// </summary>
        public ILogger Logger { protected get; set; }

        /// <summary>
        ///     Reference to the object to object mapper.
        /// </summary>
        public IObjectMapper ObjectMapper { get; set; }

        /// <summary>
        ///     Messagebus for publishing to a queue mechanism
        /// </summary>
        public IMessageBus MessageBus { get; set; }

        public void UseUow(Action act)
        {
            using (IUnitOfWorkCompleteHandle uow = UnitOfWorkManager.Begin())
            {
                act();

                uow.Complete();
            }
        }

        protected void UseUow(Action act, IsolationLevel isolation)
        {
            using (IUnitOfWorkCompleteHandle uow = UnitOfWorkManager.Begin(new UnitOfWorkOptions { IsolationLevel = isolation }))
            {
                act();

                uow.Complete();
            }
        }

        protected void UseUow(Action act, bool isTransactional)
        {
            using (IUnitOfWorkCompleteHandle uow = UnitOfWorkManager.Begin(new UnitOfWorkOptions { IsTransactional = isTransactional }))
            {
                act();

                uow.Complete();
            }
        }

        protected void UseUow(Action act, TransactionScopeOption scope)
        {
            using (IUnitOfWorkCompleteHandle uow = UnitOfWorkManager.Begin(new UnitOfWorkOptions
            {
                Scope = scope
            }))
            {
                act();

                uow.Complete();
            }
        }

        protected void UseUow(Action act, IsolationLevel isolation, TransactionScopeOption scope)
        {
            using (IUnitOfWorkCompleteHandle uow = UnitOfWorkManager.Begin(new UnitOfWorkOptions
            {
                IsolationLevel = isolation,
                Scope = scope
            }))
            {
                act();

                uow.Complete();
            }
        }

        protected void UseUow(Action act, Action<UnitOfWorkOptions> optsAction)
        {
            var options = new UnitOfWorkOptions();

            optsAction(options);

            using (IUnitOfWorkCompleteHandle uow = UnitOfWorkManager.Begin(options))
            {
                act();

                uow.Complete();
            }
        }

        protected void UseUowIfNot(Action act)
        {
            if (UnitOfWorkManager.Current == null)
            {
                using (IUnitOfWorkCompleteHandle uow = UnitOfWorkManager.Begin())
                {
                    act();

                    uow.Complete();
                }
            }
            else
            {
                act();
            }
        }

        protected void UseUowIfNot(Action act, bool isTransactional)
        {
            if (UnitOfWorkManager.Current == null)
            {
                using (IUnitOfWorkCompleteHandle uow = UnitOfWorkManager.Begin(new UnitOfWorkOptions
                {
                    IsTransactional = isTransactional
                }))
                {
                    act();

                    uow.Complete();
                }
            }
            else
            {
                act();
            }
        }

        protected void UseUowIfNot(Action act, IsolationLevel isolation)
        {
            if (UnitOfWorkManager.Current == null)
            {
                using (IUnitOfWorkCompleteHandle uow = UnitOfWorkManager.Begin(new UnitOfWorkOptions
                {
                    IsolationLevel = isolation
                }))
                {
                    act();

                    uow.Complete();
                }
            }
            else
            {
                act();
            }
        }

        protected void UseUowIfNot(Action act, IsolationLevel isolation, TransactionScopeOption scope)
        {
            if (UnitOfWorkManager.Current == null)
            {
                using (IUnitOfWorkCompleteHandle uow = UnitOfWorkManager.Begin(new UnitOfWorkOptions
                {
                    IsolationLevel = isolation,
                    Scope = scope
                }))
                {
                    act();

                    uow.Complete();
                }
            }
            else
            {
                act();
            }
        }

        protected void UseUowIfNot(Action act, Action<UnitOfWorkOptions> optsAction)
        {
            var options = new UnitOfWorkOptions();
            optsAction(options);

            if (UnitOfWorkManager.Current == null)
            {
                using (IUnitOfWorkCompleteHandle uow = UnitOfWorkManager.Begin(options))
                {
                    act();

                    uow.Complete();
                }
            }
            else
            {
                act();
            }
        }

        protected void OnUowCompleted(Action action)
        {
            CurrentUnitOfWork.Completed += (sender, args) =>
            {
                try
                {
                    action();
                }
                catch (Exception exception)
                {
                    Logger.Fatal(exception);
                }
            };
        }

        protected IDisposable MeasurePerformance(string methodNameOrText)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            Logger.Info(methodNameOrText + " starting...");

            return new DisposeAction(() =>
            {
                stopwatch.Stop();
                Logger.Info($"{methodNameOrText} takes {stopwatch.Elapsed.Minutes} minutes, {stopwatch.Elapsed.Seconds} seconds, {stopwatch.Elapsed.Milliseconds} miliseconds.");
            });
        }
    }
}
