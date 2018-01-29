using System;

namespace Stove.Commands
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

        /// <summary>
        ///     Defines a scope and sets the <see cref="CommandContext" />
        /// </summary>
        /// <param name="contextCallback"></param>
        /// <returns></returns>
        IDisposable Use(Action<CommandContext> contextCallback);

        /// <summary>
        ///     Changes the current <see cref="CommandContext" />
        /// </summary>
        /// <param name="contextCallback"></param>
        void Manipulate(Action<CommandContext> contextCallback);
    }
}
