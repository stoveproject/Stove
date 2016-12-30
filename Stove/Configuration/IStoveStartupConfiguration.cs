using Autofac.Extras.IocManager;

using Stove.Domain.Uow;

namespace Stove.Configuration
{
    public interface IStoveStartupConfiguration
    {
        IIocManager IocManager { get; }

        /// <summary>
        ///     Gets/sets default connection string used by ORM module.
        ///     It can be name of a connection string in application's config file or can be full connection string.
        /// </summary>
        string DefaultNameOrConnectionString { get; set; }

        /// <summary>
        ///     Used to configure unit of work defaults.
        /// </summary>
        IUnitOfWorkDefaultOptions UnitOfWork { get; set; }
    }
}
