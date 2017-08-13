using System.Reflection;

using Shouldly;

using Stove.Bootstrapping;
using Stove.Reflection.Extensions;
using Stove.TestBase;

using Xunit;

namespace Stove.Tests.Bootstrapping
{
    public class Bootstrapping_Tests : TestBaseWithLocalIocResolver
    {
        [Fact]
        public void events_should_called_once()
        {
            //-----------------------------------------------------------------------------------------------------------
            // Arrange
            //-----------------------------------------------------------------------------------------------------------
            Building(builder =>
                {
                    builder
                        .UseStoveWithNullables(typeof(MyTestBootstrapper))
                        .RegisterServices(r => r.RegisterAssemblyByConvention(typeof(Bootstrapping_Tests).GetAssembly()));
                })
                .Ok();

            //-----------------------------------------------------------------------------------------------------------
            // Act
            //-----------------------------------------------------------------------------------------------------------
            var testModule = The<MyTestBootstrapper>();
            var otherModule = The<MyOtherBootstrapper>();
            var anotherModule = The<MyAnotherBootstrapper>();

            //-----------------------------------------------------------------------------------------------------------
            // Assert
            //-----------------------------------------------------------------------------------------------------------
            testModule.PreStartCount.ShouldBe(1);
            testModule.StartCount.ShouldBe(1);
            testModule.PostStartCount.ShouldBe(1);

            otherModule.PreStartCount.ShouldBe(1);
            otherModule.StartCount.ShouldBe(1);
            otherModule.PostStartCount.ShouldBe(1);

            otherModule.CallMeOnStartupCount.ShouldBe(1);

            anotherModule.PreStartCount.ShouldBe(1);
            anotherModule.StartCount.ShouldBe(1);
            anotherModule.PostStartCount.ShouldBe(1);
        }
    }

    [DependsOn(typeof(MyOtherBootstrapper))]
    [DependsOn(typeof(MyAnotherBootstrapper))]
    public class MyTestBootstrapper : MyEventCounterBootstrapperBase
    {
        private readonly MyOtherBootstrapper _otherBootstrapper;

        public MyTestBootstrapper(MyOtherBootstrapper otherBootstrapper)
        {
            _otherBootstrapper = otherBootstrapper;
        }

        public override void PreStart()
        {
            base.PreStart();
            _otherBootstrapper.PreStartCount.ShouldBe(1);
            _otherBootstrapper.CallMeOnStartup();
        }

        public override void Start()
        {
            base.Start();
            _otherBootstrapper.StartCount.ShouldBe(1);
        }

        public override void PostStart()
        {
            base.PostStart();
            _otherBootstrapper.PostStartCount.ShouldBe(1);
        }
    }

    public class MyOtherBootstrapper : MyEventCounterBootstrapperBase
    {
        public int CallMeOnStartupCount { get; private set; }

        public void CallMeOnStartup()
        {
            CallMeOnStartupCount++;
        }
    }

    public class MyAnotherBootstrapper : MyEventCounterBootstrapperBase
    {
    }

    public abstract class MyEventCounterBootstrapperBase : StoveBootstrapper
    {
        public int PreStartCount { get; private set; }

        public int StartCount { get; private set; }

        public int PostStartCount { get; private set; }

        public override void PreStart()
        {
            Resolver.ShouldNotBe(null);
            StoveConfiguration.ShouldNotBe(null);
            PreStartCount++;
        }

        public override void Start()
        {
            StartCount++;
        }

        public override void PostStart()
        {
            PostStartCount++;
        }
    }
}
