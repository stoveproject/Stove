using System;

namespace Stove.Runtime.Session
{
    /// <summary>
    ///     Defines some session information that can be useful for applications.
    /// </summary>
    public interface IStoveSession
    {
        /// <summary>
        ///     Gets current UserId or null.
        ///     It can be null if no user logged in.
        /// </summary>
        long? UserId { get; }

        /// <summary>
        ///     UserId of the impersonator.
        ///     This is filled if a user is performing actions behalf of the <see cref="UserId" />.
        /// </summary>
        long? ImpersonatorUserId { get; }

        /// <summary>
        ///     Uses the specified user identifier.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        IDisposable Use(long? userId);
    }
}
