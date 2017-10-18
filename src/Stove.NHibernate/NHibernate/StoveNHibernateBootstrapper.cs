using System.Linq;

using NHibernate.Util;

using Stove.Bootstrapping;
using Stove.NHibernate.Configuration;
using Stove.NHibernate.Interceptors;

namespace Stove.NHibernate
{
    [DependsOn(typeof(StoveKernelBootstrapper))]
    public class StoveNHibernateBootstrapper : StoveBootstrapper
    {
        public override void Start()
        {
            StoveConfiguration.GetConfigurerIfExists<IStoveNHibernateConfiguration>()(StoveConfiguration.Modules.StoveNHibernate());

            StoveConfiguration.Modules.StoveNHibernate()
                              .FluentConfigurations
                              .Select(x => x)
                              .ForEach(fCfg =>
                              {
                                  StoveConfiguration.Modules.StoveNHibernate()
                                                    .SessionFactories.Add(
                                                        fCfg.Key,
                                                        fCfg.Value.ExposeConfiguration(cfg => cfg.SetInterceptor(Resolver.Resolve<StoveNHibernateInterceptor>()))
                                                            .BuildSessionFactory()
                                                    );
                              });
        }
    }
}
