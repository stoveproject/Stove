using Autofac.Extras.IocManager;

namespace Stove.Domain.Services
{
    /// <summary>
    ///     This interface must be implemented by all domain services to identify them by convention.
    /// </summary>
    /// <seealso cref="Autofac.Extras.IocManager.ITransientDependency" />
    public interface IDomainService : ITransientDependency
    {
    }
}
