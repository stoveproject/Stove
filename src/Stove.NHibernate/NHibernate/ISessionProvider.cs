using NHibernate;

namespace Stove.NHibernate
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
