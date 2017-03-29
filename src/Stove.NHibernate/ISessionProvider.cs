using NHibernate;

namespace Stove
{
    public interface ISessionProvider
    {
        /// <summary>
        ///     Gets the session.
        /// </summary>
        /// <value>
        ///     The session.
        /// </value>
        ISession Session { get; }
    }
}
