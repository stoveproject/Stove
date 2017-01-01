namespace Stove.Runtime.Session
{
    /// <summary>
    ///     Extension methods for <see cref="IStoveSession" />.
    /// </summary>
    public static class StoveSessionExtensions
    {
        /// <summary>
        ///     Gets current User's Id.
        ///     Throws <see cref="StoveException" /> if <see cref="IStoveSession.UserId" /> is null.
        /// </summary>
        /// <param name="session">Session object.</param>
        /// <returns>Current User's Id.</returns>
        public static long GetUserId(this IStoveSession session)
        {
            if (!session.UserId.HasValue)
            {
                throw new StoveException("Session.UserId is null! Probably, user is not logged in.");
            }

            return session.UserId.Value;
        }

        /// <summary>
        ///     Creates <see cref="UserIdentifier" /> from given session.
        ///     Returns null if <see cref="IStoveSession.UserId" /> is null.
        /// </summary>
        /// <param name="session">The session.</param>
        public static UserIdentifier ToUserIdentifier(this IStoveSession session)
        {
            return session.UserId == null
                ? null
                : new UserIdentifier(session.GetUserId());
        }
    }
}
