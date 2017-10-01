using System;
using System.Reflection;

using Mapster;

using Stove.Bootstrapping;
using Stove.Reflection;

namespace Stove.Mapster
{
    [DependsOn(
        typeof(StoveKernelBootstrapper)
    )]
    public class StoveMapsterBootstrapper : StoveBootstrapper
    {
        private readonly ITypeFinder _typeFinder;

        public StoveMapsterBootstrapper(ITypeFinder typeFinder)
        {
            _typeFinder = typeFinder;
        }

        public override void PreStart()
        {
            StoveConfiguration.Modules.StoveMapster().Configurators.Add(FindAndAutoMapTypes);
        }

        public override void Start()
        {
        }

        public override void PostStart()
        {
            StoveConfiguration.Modules
                         .StoveMapster()
                         .Configurators
                         .ForEach(confgurerAction =>
                         {
                             confgurerAction(StoveConfiguration.Modules.StoveMapster().Configuration);
                         });

            StoveConfiguration.Modules.StoveMapster().Configuration.Compile();
        }

        private void FindAndAutoMapTypes(TypeAdapterConfig configuration)
        {
            Type[] types = _typeFinder.Find(type =>
                type.GetTypeInfo().IsDefined(typeof(AutoMapAttribute)) ||
                type.GetTypeInfo().IsDefined(typeof(AutoMapFromAttribute)) ||
                type.GetTypeInfo().IsDefined(typeof(AutoMapToAttribute))
            );

            Logger.Debug($"Found {types.Length} classes define auto mapping attributes");

            foreach (Type type in types)
            {
                Logger.Debug(type.FullName);
                configuration.CreateAutoAttributeMaps(type);
            }
        }
    }
}
