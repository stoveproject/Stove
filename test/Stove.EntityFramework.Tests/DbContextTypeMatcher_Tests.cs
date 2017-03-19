using NSubstitute;

using Shouldly;

using Stove.Domain.Entities;
using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.EntityFramework.Repositories;
using Stove.TestBase;

using Xunit;

namespace Stove.EntityFramework.Tests
{
    public class DbContextTypeMatcher_Tests : TestBaseWithLocalIocResolver
    {
        private readonly DbContextTypeMatcher matcher;

        public DbContextTypeMatcher_Tests()
        {
            var fakeUow = Substitute.For<IUnitOfWork>();

            var fakeCurrentUowProvider = Substitute.For<ICurrentUnitOfWorkProvider>();
            fakeCurrentUowProvider.Current.Returns(fakeUow);

            matcher = new DbContextTypeMatcher(fakeCurrentUowProvider);
            matcher.Populate(new[]
            {
                typeof(MyDerivedDbContext1),
                typeof(MyDerivedDbContext2),
                typeof(MyDerivedDbContext3)
            });
        }

        [Fact]
        public void Should_Get_Same_Types_For_Defined_Non_Abstract_Types()
        {
            matcher.GetConcreteType(typeof(MyDerivedDbContext1)).ShouldBe(typeof(MyDerivedDbContext1));
            matcher.GetConcreteType(typeof(MyDerivedDbContext2)).ShouldBe(typeof(MyDerivedDbContext2));
            matcher.GetConcreteType(typeof(MyDerivedDbContext3)).ShouldBe(typeof(MyDerivedDbContext3));
        }

        [Fact]
        public void Should_Get_Same_Types_For_Undefined_Non_Abstract_Types()
        {
            matcher.GetConcreteType(typeof(MyDerivedDbContextNotDefined)).ShouldBe(typeof(MyDerivedDbContextNotDefined));
        }

        private abstract class MyCommonDbContext : StoveDbContext
        {
        }

        private class MyDerivedDbContext1 : MyCommonDbContext
        {
        }

        [AutoRepositoryTypes( //Does not matter parameters for these tests
            typeof(IRepository<>),
            typeof(IRepository<,>),
            typeof(EfRepositoryBase<,>),
            typeof(EfRepositoryBase<,,>)
        )]
        private class MyDerivedDbContext2 : MyCommonDbContext
        {
        }

        private class MyDerivedDbContext3 : MyCommonDbContext
        {
        }

        private class MyDerivedDbContextNotDefined : MyCommonDbContext
        {
        }

        private class MyCommonEntity : Entity
        {
        }
    }
}
