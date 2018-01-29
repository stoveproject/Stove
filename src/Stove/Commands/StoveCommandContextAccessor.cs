using System;

using Autofac.Extras.IocManager;

using Stove.Runtime;

namespace Stove.Commands
{
    public class StoveCommandContextAccessor : IStoveCommandContextAccessor, ITransientDependency
    {
        private const string CommandContextKey = "Stove.Commands.Context";

        private readonly IAmbientScopeProvider<CommandContext> _commandScopeProvider;

        public StoveCommandContextAccessor(IAmbientScopeProvider<CommandContext> commandScopeProvider)
        {
            _commandScopeProvider = commandScopeProvider;
        }

        public IDisposable Use(string correlationId)
        {
            return _commandScopeProvider.BeginScope(CommandContextKey, new CommandContext
            {
                CorrelationId = correlationId
            });
        }

        public IDisposable Use(Action<CommandContext> contextCallback)
        {
            var ctx = new CommandContext();
            contextCallback(ctx);
            return _commandScopeProvider.BeginScope(CommandContextKey, ctx);
        }

        public void Manipulate(Action<CommandContext> contextCallback)
        {
            contextCallback(CommandContext);
        }

        public CommandContext CommandContext => _commandScopeProvider.GetValue(CommandContextKey);
    }
}
