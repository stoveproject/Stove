using Stove.Bootstrapping;
using Stove.Demo.ConsoleApp.DbContexes;
using Stove.EntityFramework.Bootstrappers;
using Stove.Hangfire.Bootsrappers;

namespace Stove.Demo.ConsoleApp
{
    [DependsOn(
        typeof(StoveEntityFrameworkBootstrapper),
        typeof(StoveHangFireBootstrapper)
        )]
    public class StoveDemoBootstrapper : StoveBootstrapper
    {
        public override void Start()
        {
            Configuration.TypedConnectionStrings.Add(typeof(AnimalStoveDbContext), "Default");
            Configuration.TypedConnectionStrings.Add(typeof(PersonStoveDbContext), "Default");
        }
    }
}
