using System;

using Autofac.Extras.IocManager;

namespace Stove.TestBase
{
    public abstract class TestBaseWithLocalIocResolver
    {
        protected IIocBuilder IocBuilder;
        protected IRootResolver LocalResolver;

        protected TestBaseWithLocalIocResolver()
        {
            IocBuilder = Autofac.Extras.IocManager.IocBuilder.New
                                .UseAutofacContainerBuilder();
        }

        protected void Building(Action<IIocBuilder> builderAction)
        {
            builderAction(IocBuilder);
            LocalResolver = IocBuilder.CreateResolver();
        }
    }
}
