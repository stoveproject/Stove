using System;

namespace Stove.Events.Bus.Exceptions
{
    /// <summary>
    /// This type of events are used to notify for exceptions handled by Stove infrastructure.
    /// </summary>
    public class StoveHandledExceptionData : ExceptionData
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="exception">Exception object</param>
        public StoveHandledExceptionData(Exception exception)
            : base(exception)
        {

        }
    }
}