using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using Stove.Commands;
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

        /// <summary>
        ///     The <see cref="CommandContext" /> accessor
        /// </summary>
        public IStoveCommandContextAccessor CommandContextAccessor { get; set; }

        protected Task<TResponse> CorrelatingBy<TResponse>(Func<Task<TResponse>> func, string correlationId)
        {
            using (CommandContextAccessor.Use(correlationId))
            {
                return func();
            }
        }

        protected async Task UseUow(Func<Task> func, Action<UnitOfWorkOptions> optsAction = null, CancellationToken cancellationToken = default)
        {
            var options = new UnitOfWorkOptions();

            optsAction?.Invoke(options);

            using (IUnitOfWorkCompleteHandle uow = UnitOfWorkManager.Begin(options))
            {
                await func();

                await uow.CompleteAsync(cancellationToken);
            }
        }

        protected async Task<TResponse> UseUow<TResponse>(Func<Task<TResponse>> func, Action<UnitOfWorkOptions> optsAction = null, CancellationToken cancellationToken = default)
        {
            var options = new UnitOfWorkOptions();
            optsAction?.Invoke(options);

            TResponse response;
            using (IUnitOfWorkCompleteHandle uow = UnitOfWorkManager.Begin(options))
            {
                response = await func();

                await uow.CompleteAsync(cancellationToken);
            }

            return response;
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
