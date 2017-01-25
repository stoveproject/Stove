using Stove.Demo.ConsoleApp.Entities;
using Stove.Mapster.Mapster;

namespace Stove.Demo.ConsoleApp.Dto
{
    [AutoMapTo(typeof(PersonAnimal))]
    public class PersonAnimalDto
    {
        public string PersonName { get; set; }

        public string AnimalName { get; set; }
    }
}
