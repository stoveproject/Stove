using Shouldly;

using Stove.Dapper.Repositories;
using Stove.Dapper.Tests.Entities;
using Stove.Domain.Repositories;
using Stove.Domain.Uow;

using Xunit;

namespace Stove.Dapper.Tests
{
    public class Dapper_RepositoryTests : StoveDapperApplicationTestBase
    {
        public Dapper_RepositoryTests()
        {
            Building(builder => { }).Ok();
        }

        [Fact]
        public void when_conn_is_not_open_for_dapperRepo_it_opens_itself()
        {
            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin(new UnitOfWorkOptions
            {
                IsTransactional = false
            }))
            {
                The<IDapperRepository<Product>>().Insert(new Product("Pant-1"));
                The<IDapperRepository<Product>>().GetAll(x => x.Name == "Pant-1");

                Product product = The<IRepository<Product>>().FirstOrDefault(x => x.Name == "Pant-1");
                product.ShouldNotBeNull();

                uow.Complete();
            }
        }
    }
}
