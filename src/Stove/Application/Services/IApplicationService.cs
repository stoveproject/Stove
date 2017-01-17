using Autofac.Extras.IocManager;

namespace Stove.Application.Services
{
    /// <summary>
    ///     This interface must be implemented by all application services to identify them by convention.
    /// </summary>
    /// <seealso cref="Autofac.Extras.IocManager.ITransientDependency" />
    public interface IApplicationService : ITransientDependency
    {
    }
}
