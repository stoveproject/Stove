using System;

using Autofac.Extras.IocManager;

namespace Stove.TestBase
{
    public abstract class TestBaseWithLocalIocResolver : IDisposable
    {
        protected IIocBuilder IocBuilder;
        protected IRootResolver LocalResolver;

        protected TestBaseWithLocalIocResolver()
        {
            IocBuilder = Autofac.Extras.IocManager.IocBuilder.New.UseAutofacContainerBuilder();
        }

        protected TestBaseWithLocalIocResolver Building(Action<IIocBuilder> builderAction)
        {
            builderAction(IocBuilder);
            return this;
        }

        public void Ok(bool ignoreStartableComponents = false)
        {
            LocalResolver = IocBuilder.CreateResolver(ignoreStartableComponents);
        }

        public void Dispose()
        {
            LocalResolver?.Dispose();
        }
    }
}
