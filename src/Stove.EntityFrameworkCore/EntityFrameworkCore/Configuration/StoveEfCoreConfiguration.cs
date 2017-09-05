using System;

using Autofac;
using Autofac.Extras.IocManager;

using Microsoft.EntityFrameworkCore;

namespace Stove.EntityFrameworkCore.Configuration
{
    public class StoveEfCoreConfiguration : IStoveEfCoreConfiguration, ISingletonDependency
    {
         
    }
}
