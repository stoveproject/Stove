using Autofac.Extras.IocManager;

using Stove.BackgroundJobs;

namespace Stove.Demo.BackgroundJobs
{
    public class SimpleBackgroundJob : BackgroundJob<SimpleBackgroundJobArgs>, ITransientDependency
    {
        private readonly ISimpleDependency _simpleDependency;

        public SimpleBackgroundJob(ISimpleDependency simpleDependency)
        {
            _simpleDependency = simpleDependency;
        }

        public override void Execute(SimpleBackgroundJobArgs args)
        {
            string message = args.Message;
        }
    }
}
