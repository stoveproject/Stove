using System;

namespace Stove
{
    /// <summary>
    ///     Used to generate Ids.
    /// </summary>
    public interface IGuidGenerator
    {
        /// <summary>
        ///     Creates a GUID.
        /// </summary>
        Guid Create();
    }
}
