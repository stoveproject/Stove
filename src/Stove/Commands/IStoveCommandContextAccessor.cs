using System;

namespace Stove
{
    public interface IStoveCommandContextAccessor
    {
        /// <summary>
        ///     Command context
        /// </summary>
        CommandContext CommandContext { get; }

        /// <summary>
        ///     Defines a scope and sets correlationId to the <see cref="CommandContext" />
        /// </summary>
        /// <param name="correlationId"></param>
        /// <returns></returns>
        IDisposable Use(string correlationId);
    }
}
