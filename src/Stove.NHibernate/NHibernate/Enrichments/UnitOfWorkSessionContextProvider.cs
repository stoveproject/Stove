using NHibernate;

using Stove.Domain.Uow;
using Stove.NHibernate.Uow;

namespace Stove.NHibernate.Enrichments
{
    public class UnitOfWorkSessionContextProvider<TSessionContext> : ISessionContextProvider<TSessionContext> where TSessionContext : StoveSessionContext
    {
        private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;

        public UnitOfWorkSessionContextProvider(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
        {
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
        }

        public ISession GetSession()
        {
            return _currentUnitOfWorkProvider.Current.GetSession<TSessionContext>();
        }
    }
}
