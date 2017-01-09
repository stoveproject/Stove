using Stove.Bootstrapping;

namespace Stove.Demo.ConsoleApp.DbContexes
{
    public class DbContextBootstrapper : StoveBootstrapper
    {
        public override void Start()
        {
            Configuration.TypedConnectionStrings.Add(typeof(AnimalStoveDbContext), "Default");
            Configuration.TypedConnectionStrings.Add(typeof(PersonStoveDbContext), "Default");
        }
    }
}
