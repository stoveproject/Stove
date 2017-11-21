using System;
using System.Collections.Generic;

using FluentNHibernate.Cfg;

using NHibernate;

using Stove.Configuration;

namespace Stove.NHibernate.Configuration
{
    public interface IStoveNHibernateConfiguration
    {
        /// <summary>
        ///     Used to get and modify NHibernate fluent configuration.
        ///     You can add mappings to this object.
        ///     Do not call BuildSessionFactory on it.
        /// </summary>

        Dictionary<Type, FluentConfiguration> FluentConfigurations { get; set; }

        Dictionary<Type, ISessionFactory> SessionFactories { get; }

        IStoveStartupConfiguration Configuration { get; }

        IStoveNHibernateConfiguration AddFluentConfigurationFor<TStoveSessionContext>(Func<FluentConfiguration> cfgFactory);
    }
}
