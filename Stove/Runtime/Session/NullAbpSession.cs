namespace Stove.Runtime.Session
{
    /// <summary>
    ///     Implements null object pattern for <see cref="IStoveSession" />.
    /// </summary>
    public class NullStoveSession : IStoveSession
    {
        private NullStoveSession()
        {
        }

        /// <summary>
        ///     Singleton instance.
        /// </summary>
        public static NullStoveSession Instance { get; } = new NullStoveSession();

        /// <inheritdoc />
        public long? UserId => null;

        public long? ImpersonatorUserId => null;
    }
}
