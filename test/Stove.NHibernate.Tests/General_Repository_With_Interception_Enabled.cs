using System.Collections.Generic;

using Shouldly;

using Stove.Domain.Repositories;
using Stove.NHibernate.Tests.Entities;
using Stove.NHibernate.Tests.Sessions;

using Xunit;

namespace Stove.NHibernate.Tests
{
    public class General_Repository_With_Interception_Enabled : StoveNHibernateTestBase
    {
        public General_Repository_With_Interception_Enabled() : base(true)
        {
            Building(builder => { }).Ok();
            StoveSession.UserId = 1;
            UsingSession<PrimaryStoveSessionContext>(session => { session.Save(new Product("TShirt")); });
        }

        [Fact]
        public async void GetAllAsync_should_work()
        {
            List<Product> products = await The<IRepository<Product>>().GetAllListAsync();
            products.Count.ShouldBe(1);
        }
    }
}
