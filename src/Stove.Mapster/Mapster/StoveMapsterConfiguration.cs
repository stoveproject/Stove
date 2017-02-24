using System;
using System.Collections.Generic;

using Mapster;

namespace Stove.Mapster.Mapster
{
    public class StoveMapsterConfiguration : IStoveMapsterConfiguration
    {
        public StoveMapsterConfiguration()
        {
            Configurators = new List<Action<TypeAdapterConfig>>();
            Configuration = new TypeAdapterConfig();
            Adapter = new Adapter(Configuration);
        }

        public TypeAdapterConfig Configuration { get; }

        public List<Action<TypeAdapterConfig>> Configurators { get; }

        public Adapter Adapter { get; }
    }
}
