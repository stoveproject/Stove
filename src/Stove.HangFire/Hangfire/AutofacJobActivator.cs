using System;

using Autofac;

using Hangfire;
using Hangfire.Annotations;

namespace Stove.Hangfire.Hangfire
{
    /// <summary>
    ///     Hangfire Job Activator based on Autofac IoC Container.
    /// </summary>
    public class AutofacJobActivator : JobActivator
    {
        /// <summary>
        ///     Tag used in setting up per-job lifetime scope registrations.
        /// </summary>
        public static readonly object LifetimeScopeTag = "BackgroundJobScope";

        private readonly ILifetimeScope _lifetimeScope;
        private readonly bool _useTaggedLifetimeScope;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AutofacJobActivator" />
        ///     class with the given Autofac Lifetime Scope.
        /// </summary>
        /// <param name="lifetimeScope">
        ///     Container that will be used to create instance
        ///     of classes during job activation process.
        /// </param>
        /// <param name="useTaggedLifetimeScope">
        ///     Should the Container use Tag
        ///     BackgroundJobScope to resolve dependencies or create new Scope on each job
        /// </param>
        public AutofacJobActivator([NotNull] ILifetimeScope lifetimeScope, bool useTaggedLifetimeScope = true)
        {
            Check.NotNull(lifetimeScope, nameof(lifetimeScope));

            _lifetimeScope = lifetimeScope;
            _useTaggedLifetimeScope = useTaggedLifetimeScope;
        }

        /// <inheritdoc />
        public override object ActivateJob(Type jobType)
        {
            return _lifetimeScope.Resolve(jobType);
        }

        public override JobActivatorScope BeginScope()
        {
            return new AutofacScope(_useTaggedLifetimeScope
                ? _lifetimeScope.BeginLifetimeScope(LifetimeScopeTag)
                : _lifetimeScope.BeginLifetimeScope());
        }

        private class AutofacScope : JobActivatorScope
        {
            private readonly ILifetimeScope _lifetimeScope;

            public AutofacScope(ILifetimeScope lifetimeScope)
            {
                _lifetimeScope = lifetimeScope;
            }

            public override object Resolve(Type type)
            {
                return _lifetimeScope.Resolve(type);
            }

            public override void DisposeScope()
            {
                _lifetimeScope.Dispose();
            }
        }
    }
}
