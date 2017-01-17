using System;
using System.Collections.Generic;

using Mapster;

namespace Stove.Mapster.Mapster
{
    public class StoveMapsterConfiguration : IStoveMapsterConfiguraiton
    {
        public StoveMapsterConfiguration()
        {
            Configurators = new List<Action<TypeAdapterConfig>>();
            Configuration = TypeAdapterConfig.GlobalSettings;
        }

        public TypeAdapterConfig Configuration { get; }

        public List<Action<TypeAdapterConfig>> Configurators { get; }
    }
}
