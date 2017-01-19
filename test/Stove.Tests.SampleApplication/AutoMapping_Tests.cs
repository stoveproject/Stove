using Shouldly;

using Stove.ObjectMapping;
using Stove.Tests.SampleApplication.Dtos;

using Xunit;

namespace Stove.Tests.SampleApplication
{
    public class AutoMapping_Tests : SampleApplicationTestBase<SampleApplicationBootstrapper>
    {
        [Fact]
        public void auto_object_mapping_should_work()
        {
            Building(builder => { }).Ok();

            var mapper = LocalResolver.Resolve<IObjectMapper>();

            var person = new Person { Name = "Oğuzhan" };

            var personDto = mapper.Map<PersonDto>(person);
            var personEntity = mapper.Map<Person>(personDto);

            personDto.ShouldNotBeNull();
            personEntity.ShouldNotBeNull();
        }
    }
}
