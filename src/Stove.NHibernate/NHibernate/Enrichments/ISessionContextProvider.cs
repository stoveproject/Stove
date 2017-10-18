using NHibernate;

namespace Stove.NHibernate.Enrichments
{
    public interface ISessionContextProvider<TSessionContext> where TSessionContext : StoveSessionContext
    {
        ISession GetSession();
    }
}
