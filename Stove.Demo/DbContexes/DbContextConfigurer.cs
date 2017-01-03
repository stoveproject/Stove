using Stove.Configuration.Configurers;

namespace Stove.Demo.DbContexes
{
    public class DbContextConfigurer : StoveConfigurer
    {
        public override void Start()
        {
            Configuration.TypedConnectionStrings.Add(typeof(AnimalStoveDbContext), "Default");
            Configuration.TypedConnectionStrings.Add(typeof(PersonStoveDbContext), "Default");
        }
    }
}
