using System;
using System.Reflection;

using Mapster;

using Stove.Bootstrapping;
using Stove.Mapster.Mapster;
using Stove.Reflection;

namespace Stove.Mapster.Bootstrappers
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
            Configuration.Modules.StoveMapster().Configurators.Add(FindAndAutoMapTypes);
        }

        public override void Start()
        {
        }

        public override void PostStart()
        {
            Configuration.Modules
                         .StoveMapster()
                         .Configurators
                         .ForEach(confgurerAction =>
                         {
                             confgurerAction(Configuration.Modules.StoveMapster().Configuration);
                         });

            Configuration.Modules.StoveMapster().Configuration.Compile();
        }

        private void FindAndAutoMapTypes(TypeAdapterConfig configuration)
        {
            Type[] types = _typeFinder.Find(type =>
                type.IsDefined(typeof(AutoMapAttribute)) ||
                type.IsDefined(typeof(AutoMapFromAttribute)) ||
                type.IsDefined(typeof(AutoMapToAttribute))
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
