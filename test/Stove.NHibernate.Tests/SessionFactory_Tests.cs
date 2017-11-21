using NHibernate;

using Stove.NHibernate.Enrichments;
using Stove.NHibernate.Tests.Sessions;

using Xunit;

namespace Stove.NHibernate.Tests
{
    public class SessionFactory_Test : StoveNHibernateTestBase
    {
        private readonly ISessionFactoryProvider _sessionFactoryProvider;

        public SessionFactory_Test()
        {
            Building(builder => { }).Ok();

            _sessionFactoryProvider = The<ISessionFactoryProvider>();
        }

        [Fact]
        public void Should_OpenSession_Work()
        {
            using (ISession session = _sessionFactoryProvider.GetSessionFactory<PrimaryStoveSessionContext>().OpenSession())
            {
            }
        }
    }
}
