using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.Internal;

using Shouldly;

using Stove.Configuration;
using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.Mapster;
using Stove.ObjectMapping;
using Stove.Tests.SampleApplication.Domain.Entities;
using Stove.Tests.SampleApplication.Dtos;
using Stove.Timing;

using Xunit;

namespace Stove.Tests.SampleApplication
{
    public class AutoMapping_Tests : SampleApplicationTestBase
    {
        [Fact]
        public void auto_object_mapping_should_work()
        {
            Building(builder => { }).Ok();

            var mapper = The<IObjectMapper>();

            var person = new Person { Name = "Oğuzhan" };

            var personDto = mapper.Map<PersonDto>(person);
            var personEntity = mapper.Map<Person>(personDto);

            personDto.ShouldNotBeNull();
            personEntity.ShouldNotBeNull();
        }

        [Fact]
        public void proxied_object_can_mapped_through_objectmapper_with_required_explicit_mapping()
        {
            Building(builder => { }).Ok();

            The<IModuleConfigurations>().StoveMapster().Configuration.RequireExplicitMapping = true;

            var mapper = The<IObjectMapper>();

            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                for (int i = 0; i < 10; i++)
                {
                    User user = The<IRepository<User>>().Insert(new User()
                    {
                        CreationTime = Clock.Now,
                        Name = $"Oğuzhan{i}",
                        Surname = $"Soykan{i}",
                        Email = $"oguzhansoykan@outlook.com{i}"
                    });

                    var userDto = mapper.Map<UserDto>(user);
                    userDto.ShouldNotBeNull();
                }

                uow.Complete();
            }

            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                List<User> users = The<IRepository<User>>().GetAllList();

                users.ForEach(user =>
                {
                    var userDto = mapper.Map<UserDto>(user);
                    userDto.ShouldNotBeNull();
                });

                uow.Complete();
            }
        }
    }
}
