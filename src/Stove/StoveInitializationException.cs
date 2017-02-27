using System;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace Stove
{
    /// <summary>
    ///     This exception is thrown if a problem on Stove initialization progress.
    /// </summary>
    [Serializable]
    public class StoveInitializationException : StoveException
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        public StoveInitializationException()
        {
        }

        /// <summary>
        ///     Constructor for serializing.
        /// </summary>
        public StoveInitializationException([NotNull] SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        public StoveInitializationException([NotNull] string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public StoveInitializationException([NotNull] string message, [CanBeNull] Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
