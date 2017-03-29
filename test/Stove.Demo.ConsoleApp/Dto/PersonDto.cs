using Stove.Demo.ConsoleApp.Entities;
using Stove.Mapster;

namespace Stove.Demo.ConsoleApp.Dto
{
    [AutoMapFrom(typeof(Person))]
    public class PersonDto
    {
    }
}
