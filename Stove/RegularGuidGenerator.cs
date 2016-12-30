using System;

using Autofac.Extras.IocManager;

namespace Stove
{
    /// <summary>
    ///     Implements <see cref="IGuidGenerator" /> by using <see cref="Guid.NewGuid" />.
    /// </summary>
    public class RegularGuidGenerator : IGuidGenerator, ITransientDependency
    {
        public virtual Guid Create()
        {
            return Guid.NewGuid();
        }
    }
}
