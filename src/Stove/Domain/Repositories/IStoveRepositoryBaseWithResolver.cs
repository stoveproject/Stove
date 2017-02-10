using Autofac.Extras.IocManager;

namespace Stove.Domain.Repositories
{
    public interface IStoveRepositoryBaseWithResolver
    {
        IScopeResolver ScopeResolver { get; set; }
    }
}
