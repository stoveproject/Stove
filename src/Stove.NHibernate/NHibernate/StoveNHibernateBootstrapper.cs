using Stove.Bootstrapping;

namespace Stove.NHibernate
{
    [DependsOn(typeof(StoveKernelBootstrapper))]
    public class StoveNHibernateBootstrapper : StoveBootstrapper
    {
    }
}
