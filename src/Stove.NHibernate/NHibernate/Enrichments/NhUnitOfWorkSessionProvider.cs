using Autofac.Extras.IocManager;

using NHibernate;

using Stove.Domain.Uow;
using Stove.NHibernate.Uow;

namespace Stove.NHibernate.Enrichments
{
    public class NhUnitOfWorkSessionProvider : ISessionProvider, ITransientDependency
    {
        private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;

        public NhUnitOfWorkSessionProvider(ICurrentUnitOfWorkProvider currentUnitOfWorkProvider)
        {
            _currentUnitOfWorkProvider = currentUnitOfWorkProvider;
        }

        public ISession GetSession<TSessionContext>() where TSessionContext : StoveSessionContext
        {
            return _currentUnitOfWorkProvider.Current.GetSession<TSessionContext>();
        }
    }
}
