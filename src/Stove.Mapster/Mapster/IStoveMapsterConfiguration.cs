using System;
using System.Collections.Generic;

using Mapster;

namespace Stove.Mapster.Mapster
{
    public interface IStoveMapsterConfiguration
    {
        TypeAdapterConfig Configuration { get; }

        List<Action<TypeAdapterConfig>> Configurators { get; }

        Adapter Adapter { get; }
    }
}
