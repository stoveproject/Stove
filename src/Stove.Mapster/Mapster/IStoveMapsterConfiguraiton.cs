using System;
using System.Collections.Generic;

using Mapster;

namespace Stove.Mapster.Mapster
{
    public interface IStoveMapsterConfiguraiton
    {
        TypeAdapterConfig Configuration { get; }

        List<Action<TypeAdapterConfig>> Configurators { get; }
    }
}
