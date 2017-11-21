using System;
using System.Collections.Generic;

using Autofac.Extras.IocManager;

using FluentNHibernate.Cfg;

using NHibernate;

using Stove.Configuration;

namespace Stove.NHibernate.Configuration
{
    public class StoveNHibernateConfiguration : IStoveNHibernateConfiguration, ISingletonDependency
    {
        public StoveNHibernateConfiguration(IStoveStartupConfiguration configuration)
        {
            Configuration = configuration;

            SessionFactories = new Dictionary<Type, ISessionFactory>();
            FluentConfigurations = new Dictionary<Type, FluentConfiguration>();
        }

        public Dictionary<Type, FluentConfiguration> FluentConfigurations { get; set; }

        public Dictionary<Type, ISessionFactory> SessionFactories { get; }

        public IStoveStartupConfiguration Configuration { get; }

        public IStoveNHibernateConfiguration AddFluentConfigurationFor<TStoveSessionContext>(Func<FluentConfiguration> cfgFactory)
        {
            FluentConfigurations.Add(typeof(TStoveSessionContext), cfgFactory());

            return this;
        }
    }
}
