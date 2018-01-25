using System;

using Autofac.Extras.IocManager;

using Stove.Runtime;

namespace Stove
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
            return _commandScopeProvider.BeginScope(CommandContextKey, new CommandContext(correlationId));
        }

        public CommandContext CommandContext => _commandScopeProvider.GetValue(CommandContextKey);
    }
}
