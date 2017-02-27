using JetBrains.Annotations;

using Stove.Domain.Uow;
using Stove.Log;

namespace Stove.BackgroundJobs
{
    /// <summary>
    ///     Base class that can be used to implement <see cref="IBackgroundJob{TArgs}" />.
    /// </summary>
    public abstract class BackgroundJob<TArgs> : IBackgroundJob<TArgs>
    {
        private IUnitOfWorkManager _unitOfWorkManager;

        /// <summary>
        ///     Constructor.
        /// </summary>
        protected BackgroundJob()
        {
            Logger = NullLogger.Instance;
        }

        /// <summary>
        ///     Reference to <see cref="IUnitOfWorkManager" />.
        /// </summary>
        [NotNull]
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
            set { _unitOfWorkManager = value; }
        }

        /// <summary>
        ///     Gets current unit of work.
        /// </summary>
        [CanBeNull]
        protected IActiveUnitOfWork CurrentUnitOfWork
        {
            get { return UnitOfWorkManager.Current; }
        }

        /// <summary>
        ///     Reference to the logger to write logs.
        /// </summary>
        [NotNull]
        public ILogger Logger { protected get; set; }

        public abstract void Execute(TArgs args);
    }
}
