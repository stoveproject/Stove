using System;

using Autofac.Extras.IocManager;

namespace Stove.Tests
{
    public abstract class TestBaseWithLocalIoc
    {
        protected IIocBuilder IocBuilder;
        protected IRootResolver LocalResolver;

        protected TestBaseWithLocalIoc()
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
