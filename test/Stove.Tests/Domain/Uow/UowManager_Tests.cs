using System.Transactions;

using Autofac.Extras.IocManager;

using NSubstitute;

using Stove.Domain.Uow;
using Stove.TestBase;

using Xunit;

namespace Stove.Tests.Domain.Uow
{
    public class UowManager_Tests : TestBaseWithLocalIocResolver
    {
        [Fact]
        public void Should_Call_Uow_Methods() 
        {
            var fakeUow = Substitute.For<IUnitOfWork>();

            Building(builder =>
            {
                builder.RegisterServices(r => r.Register<IUnitOfWorkDefaultOptions, UnitOfWorkDefaultOptions>(Lifetime.Singleton));
                builder.RegisterServices(r => r.Register<ICurrentUnitOfWorkProvider, CallContextCurrentUnitOfWorkProvider>(Lifetime.Singleton));
                builder.RegisterServices(r => r.Register<IUnitOfWorkManager, UnitOfWorkManager>(Lifetime.Singleton));
                builder.RegisterServices(r => r.Register(context => fakeUow, Lifetime.Singleton));
            }).Ok();

            var uowManager = LocalResolver.Resolve<IUnitOfWorkManager>();

            //Starting the first uow
            using (IUnitOfWorkCompleteHandle uow1 = uowManager.Begin())
            {
                //so, begin will be called
                fakeUow.Received(1).Begin(Arg.Any<UnitOfWorkOptions>());

                //trying to begin a uow (not starting a new one, using the outer)
                using (IUnitOfWorkCompleteHandle uow2 = uowManager.Begin())
                {
                    //Since there is a current uow, begin is not called
                    fakeUow.Received(1).Begin(Arg.Any<UnitOfWorkOptions>());

                    uow2.Complete();

                    //complete has no effect since outer uow should complete it
                    fakeUow.DidNotReceive().Complete();
                }

                //trying to begin a uow (forcing to start a NEW one)
                using (IUnitOfWorkCompleteHandle uow2 = uowManager.Begin(TransactionScopeOption.RequiresNew))
                {
                    //So, begin is called again to create an inner uow
                    fakeUow.Received(2).Begin(Arg.Any<UnitOfWorkOptions>());

                    uow2.Complete();

                    //And the inner uow should be completed
                    fakeUow.Received(1).Complete();
                }

                //complete the outer uow
                uow1.Complete();
            }

            fakeUow.Received(2).Complete();
            fakeUow.Received(2).Dispose();
        }
    }
}
