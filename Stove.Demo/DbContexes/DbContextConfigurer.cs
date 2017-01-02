using Stove.Configuration.Configurers;

namespace Stove.Demo.DbContexes
{
    public class DbContextConfigurer : StoveConfigurer
    {
        public override void Start()
        {
            Configuration.TypedConnectionStrings.Add(typeof(AnimalStoveDbContext), "Animal");
            Configuration.TypedConnectionStrings.Add(typeof(PersonStoveDbContext), "Person");
        }
    }
}
