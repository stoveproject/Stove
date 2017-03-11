using Autofac.Extras.IocManager;

using Stove.BackgroundJobs;

namespace Stove.Hangfire.Tests
{
    public class SimpleJob : BackgroundJob<SimpleJobArgs>, ISingletonDependency
    {
        public int ExecutionCount { get; set; }

        public override void Execute(SimpleJobArgs args)
        {
            string obj = args.Name;
            ExecutionCount++;
        }
    }

    public class SimpleJobArgs
    {
        public string Name { get; set; }
    }
}
