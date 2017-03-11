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

        public void Dispose()
        {
            LocalResolver?.Dispose();
        }

        protected TestBaseWithLocalIocResolver Building(Action<IIocBuilder> builderAction)
        {
            builderAction(IocBuilder);
            return this;
        }

        public void Ok(bool ignoreStartableComponents = false)
        {
            PreBuild();
            LocalResolver = IocBuilder.CreateResolver(ignoreStartableComponents);
            PostBuild();
        }

        protected virtual void PreBuild()
        {
        }

        protected virtual void PostBuild()
        {
        }

        protected T The<T>()
        {
            return LocalResolver.Resolve<T>();
        }
    }
}
