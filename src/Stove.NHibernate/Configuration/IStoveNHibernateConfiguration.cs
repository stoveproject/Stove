using FluentNHibernate.Cfg;

namespace Stove.Configuration
{
    public interface IStoveNHibernateConfiguration
    {
        /// <summary>
        ///     Used to get and modify NHibernate fluent configuration.
        ///     You can add mappings to this object.
        ///     Do not call BuildSessionFactory on it.
        /// </summary>
        FluentConfiguration FluentConfiguration { get; }

        IStoveStartupConfiguration Configuration { get; }
    }
}
