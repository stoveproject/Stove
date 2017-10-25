using NHibernate;

namespace Stove.NHibernate.Enrichments
{
    public interface ISessionProvider
    {
        ISession GetSession<TSessionContext>() where TSessionContext : StoveSessionContext;
    }
}
