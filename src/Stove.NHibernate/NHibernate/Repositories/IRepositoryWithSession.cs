using NHibernate;

namespace Stove.NHibernate.Repositories
{
    public interface IRepositoryWithSession
    {
        ISession GetSession();
    }
}
