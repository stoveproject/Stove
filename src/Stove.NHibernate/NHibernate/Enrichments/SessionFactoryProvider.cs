using Autofac.Extras.IocManager;

using NHibernate;

using Stove.NHibernate.Configuration;

namespace Stove.NHibernate.Enrichments
{
    public class SessionFactoryProvider : ISessionFactoryProvider, ITransientDependency
    {
        private readonly IStoveNHibernateConfiguration _nHibernateConfiguration;

        public SessionFactoryProvider(IStoveNHibernateConfiguration nHibernateConfiguration)
        {
            _nHibernateConfiguration = nHibernateConfiguration;
        }

        public ISessionFactory GetSessionFactory<TStoveSessionContext>() where TStoveSessionContext : StoveSessionContext
        {
            if (_nHibernateConfiguration.SessionFactories.TryGetValue(typeof(TStoveSessionContext), out ISessionFactory sessionFactory)) { return sessionFactory; }

            throw new StoveException($"Could not find {typeof(TStoveSessionContext).Name} to create SessionFactory!");
        }
    }
}
