using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

using Autofac.Extras.IocManager;

using Stove.Bootstrapping.Bootstrappers;
using Stove.Configuration;
using Stove.Log;

namespace Stove.Bootstrapping
{
    public class BootstrapperManager : IBootstrapperManager
    {
        private readonly BootstrapperCollection _bootstrappers;
        private readonly IResolver _resolver;

        public BootstrapperManager(IResolver resolver)
        {
            _resolver = resolver;
            _bootstrappers = new BootstrapperCollection();
        }

        public IReadOnlyList<BootstrapperInfo> Bootstrappers => _bootstrappers.ToImmutableList();

        public void StartBootstrappers()
        {
            LoadAllBootstrappers();

            List<BootstrapperInfo> sortedBootstrappers = _bootstrappers.GetSortedBootstrapperListByDependency();
            sortedBootstrappers.ForEach(bootstrapper => bootstrapper.Instance.PreStart());
            sortedBootstrappers.ForEach(bootstrapper => bootstrapper.Instance.Start());
            sortedBootstrappers.ForEach(bootstrapper => bootstrapper.Instance.PostStart());
        }

        private void LoadAllBootstrappers()
        {
            List<Type> bootstrappers = FindAllBootstrapperTypes();

            CreateBootstrapper(bootstrappers);

            SetDependencies();
        }

        private void CreateBootstrapper(ICollection<Type> bootstrappers)
        {
            foreach (Type boostrapperType in bootstrappers)
            {
                var bootstrapper = _resolver.Resolve(boostrapperType) as StoveBootstrapper;
                if (bootstrapper == null)
                {
                    throw new StoveInitializationException("This type is not an Stove bootstrapper: " + boostrapperType.AssemblyQualifiedName);
                }

                bootstrapper.Resolver = _resolver;
                bootstrapper.Configuration = _resolver.Resolve<IStoveStartupConfiguration>();
                bootstrapper.Logger = _resolver.Resolve<ILogger>();

                var bootstrapperInfo = new BootstrapperInfo(boostrapperType, bootstrapper);

                _bootstrappers.Add(bootstrapperInfo);
            }
        }

        private List<Type> FindAllBootstrapperTypes()
        {
            List<Type> bootstrappers = StoveBootstrapper.FindDependedBootstrapperTypesRecursivelyIncludingGivenBootstrapper(typeof(StoveKernelBootstrapper));
            return bootstrappers;
        }

        private void SetDependencies()
        {
            foreach (BootstrapperInfo bootstrapperInfo in _bootstrappers)
            {
                bootstrapperInfo.Dependencies.Clear();

                //Set dependencies for defined DependsOnAttribute attribute(s).
                foreach (Type dependedBootstrapperType in StoveBootstrapper.FindDependedBootstrapperTypes(bootstrapperInfo.Type))
                {
                    BootstrapperInfo dependedBootstrapperInfo = _bootstrappers.FirstOrDefault(m => m.Type == dependedBootstrapperType);
                    if (dependedBootstrapperInfo == null)
                    {
                        throw new StoveInitializationException("Could not find a depended bootstrapper " + dependedBootstrapperType.AssemblyQualifiedName + " for " + bootstrapperInfo.Type.AssemblyQualifiedName);
                    }

                    if (bootstrapperInfo.Dependencies.FirstOrDefault(dm => dm.Type == dependedBootstrapperType) == null)
                    {
                        bootstrapperInfo.Dependencies.Add(dependedBootstrapperInfo);
                    }
                }
            }
        }
    }
}
