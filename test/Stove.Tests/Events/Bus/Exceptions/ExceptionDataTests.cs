using Shouldly;

using Stove.Events.Bus.Exceptions;

using Xunit;

namespace Stove.Tests.Events.Bus.Exceptions
{
    public class ExceptionDataTests
    {
        [Fact]
        public void should_be_instantiatable()
        {
            var exceptionData = new ExceptionData(new StoveException("whatever"));

            exceptionData.ShouldNotBeNull();
            exceptionData.Exception.ShouldBeOfType<StoveException>();
        }
    }
}