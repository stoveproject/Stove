using System;
using System.Collections.Generic;

using Mapster;

namespace Stove.Mapster
{
    public interface IStoveMapsterConfiguration
    {
        TypeAdapterConfig Configuration { get; }

        List<Action<TypeAdapterConfig>> Configurators { get; }

        IAdapter Adapter { get; }
    }
}
