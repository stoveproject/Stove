using System;

using Stove.Threading.Timers;

namespace Stove.Threading.BackgrodunWorkers
{
    /// <summary>
    ///     Extends <see cref="BackgroundWorkerBase" /> to add a periodic running Timer.
    /// </summary>
    public abstract class PeriodicBackgroundWorkerBase : BackgroundWorkerBase
    {
        protected readonly StoveTimer Timer;

        /// <summary>
        ///     Initializes a new instance of the <see cref="PeriodicBackgroundWorkerBase" /> class.
        /// </summary>
        /// <param name="timer">A timer.</param>
        protected PeriodicBackgroundWorkerBase(StoveTimer timer)
        {
            Timer = timer;
            Timer.Elapsed += Timer_Elapsed;
        }

        public override void Start()
        {
            base.Start();
            Timer.Start();
        }

        public override void Stop()
        {
            Timer.Stop();
            base.Stop();
        }

        public override void WaitToStop()
        {
            Timer.WaitToStop();
            base.WaitToStop();
        }

        /// <summary>
        ///     Handles the Elapsed event of the Timer.
        /// </summary>
        private void Timer_Elapsed(object sender, EventArgs e)
        {
            try
            {
                DoWork();
            }
            catch (Exception ex)
            {
                Logger.Warn(ex.ToString(), ex);
            }
        }

        /// <summary>
        ///     Periodic works should be done by implementing this method.
        /// </summary>
        protected abstract void DoWork();
    }
}
