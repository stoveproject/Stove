using NHibernate;

using Xunit;

namespace Stove.NHibernate.Tests
{
    public class SessionFactory_Test : StoveNHibernateTestBase
    {
        private readonly ISessionFactory _sessionFactory;

        public SessionFactory_Test()
        {
            Building(builder => { }).Ok();

            _sessionFactory = The<ISessionFactory>();
        }

        [Fact]
        public void Should_OpenSession_Work()
        {
            using (ISession session = _sessionFactory.OpenSession())
            {
            }
        }
    }
}
