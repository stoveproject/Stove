namespace Stove.Domain.Services
{
    /// <summary>
    ///     This class can be used as a base class for domain services.
    /// </summary>
    /// <seealso cref="Stove.StoveServiceBase" />
    /// <seealso cref="Stove.Domain.Services.IDomainService" />
    public abstract class DomainService : StoveServiceBase, IDomainService
    {
    }
}
