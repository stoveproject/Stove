using NHibernate;

namespace Stove.NHibernate.Enrichments
{
    public interface ISessionFactoryProvider
    {
        ISessionFactory GetSessionFactory<TStoveSessionContext>() where TStoveSessionContext : StoveSessionContext;
    }
}
