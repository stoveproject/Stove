using Shouldly;

using Stove.Dapper.Repositories;
using Stove.Dapper.Tests.Entities;
using Stove.Domain.Uow;

namespace Stove.Dapper.Tests
{
    public class StoveDapper_Tests : StoveDapperApplicationTestBase
    {
        public StoveDapper_Tests()
        {
            Building(builder => { }).Ok();
        }

        //[Fact(Skip="Effort does not support Dapper queries")]
        public void should_work()
        {
            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                The<IDapperRepository<User>>().Insert(new User { Name = "Oğuzhan" });
                uow.Complete();
            }

            The<IDapperRepository<User>>().Get(1).ShouldNotBeNull();
        }
    }
}
