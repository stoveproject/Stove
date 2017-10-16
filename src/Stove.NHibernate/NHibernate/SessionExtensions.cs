using System.Data.Common;

using NHibernate;

namespace Stove.NHibernate
{
    public static class SessionExtensions
    {
        public static ISession OpenSessionWithConnection(this ISession session, DbConnection connection)
        {
            return session.SessionWithOptions().Connection(connection).OpenSession();
        }

        public static ISession OpenSessionWithConnection(this ISessionFactory sessionFactory, DbConnection connection)
        {
            return sessionFactory.OpenSession().OpenSessionWithConnection(connection);
        }
    }
}
