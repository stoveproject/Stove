using System;
using System.Threading.Tasks;

using Chill;

using Shouldly;

using Stove.Runtime.Remoting;

using Xunit;

namespace Stove.Tests.Commands
{
    public class StoveAsynLocalCommandContextSpecs : GivenWhenThen
    {
        private IDisposable _contextDispoer;
        private Guid _correlationId = Guid.NewGuid();

        public StoveAsynLocalCommandContextSpecs()
        {
            Given(() =>
            {
                SetThe<IStoveCommandContextAccessor>().To(
                    new StoveCommandContextAccessor(
                        new DataContextAmbientScopeProvider<CommandContext>(new AsyncLocalAmbientDataContext()
                        )));
            });

            When(() =>
            {
                _contextDispoer = The<IStoveCommandContextAccessor>().Use(_correlationId.ToString());
            });
        }

        [Fact]
        public void use_should_work()
        {
            The<IStoveCommandContextAccessor>().CommandContext.CorrelationId.ShouldBe(_correlationId.ToString());
        }

        [Fact]
        public void commandContext_should_be_null_when_disposer_invoked()
        {
            _contextDispoer.Dispose();

            The<IStoveCommandContextAccessor>().CommandContext.ShouldBeNull();
        }
    }
}
