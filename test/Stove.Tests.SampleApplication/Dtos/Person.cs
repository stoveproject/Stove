using Stove.Mapster;

namespace Stove.Tests.SampleApplication.Dtos
{
    [AutoMap(typeof(PersonDto))]
    public class Person
    {
        public string Name { get; set; }
    }
}