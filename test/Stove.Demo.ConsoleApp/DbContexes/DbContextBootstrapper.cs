using Stove.Bootstrapping;
using Stove.EntityFramework.Bootstrapper;

namespace Stove.Demo.ConsoleApp.DbContexes
{
    [DependsOn(
        typeof(DbContextTypePopulateBootstrapper)
        )]
    public class DbContextBootstrapper : StoveBootstrapper
    {
        public override void Start()
        {
            Configuration.TypedConnectionStrings.Add(typeof(AnimalStoveDbContext), "Default");
            Configuration.TypedConnectionStrings.Add(typeof(PersonStoveDbContext), "Default");
        }
    }
}
