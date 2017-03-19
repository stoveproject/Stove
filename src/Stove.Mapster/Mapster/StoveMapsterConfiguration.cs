using System;
using System.Collections.Generic;

using Mapster;

namespace Stove.Mapster
{
    public class StoveMapsterConfiguration : IStoveMapsterConfiguration
    {
        public StoveMapsterConfiguration()
        {
            Configurators = new List<Action<TypeAdapterConfig>>();
            Configuration = TypeAdapterConfig.GlobalSettings;
            Adapter = new Adapter(Configuration);
        }

        public TypeAdapterConfig Configuration { get; }

        public List<Action<TypeAdapterConfig>> Configurators { get; }

        public IAdapter  Adapter { get; }
    }
}
