using Stove.Demo.WebApi.Core.Domain.Entities;
using Stove.Mapster;

namespace Stove.Demo.WebApi.Core.Application.Dto
{
    [AutoMapTo(typeof(PersonAnimal))]
    public class PersonAnimalDto
    {
        public string PersonName { get; set; }

        public string AnimalName { get; set; }
    }
}
