using System;
using System.Linq;
using System.Transactions;

using Shouldly;

using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.Tests.SampleApplication.Domain.Entities;

using Xunit;

namespace Stove.Tests.SampleApplication.Uow
{
    public class NestedTransaction_Test : SampleApplicationTestBase
    {
        private readonly IRepository<User> _userRepository;

        public NestedTransaction_Test()
        {
            Building(builder => { }).Ok();
            _userRepository = The<IRepository<User>>();
        }

        [Fact]
        public void Should_Suppress_Outer_Transaction()
        {
            string outerUowPersonName = Guid.NewGuid().ToString("N");
            string innerUowPersonName = Guid.NewGuid().ToString("N");

            var unitOfWorkManager = The<IUnitOfWorkManager>();

            Assert.Throws<ApplicationException>(() =>
            {
                using (IUnitOfWorkCompleteHandle uow = unitOfWorkManager.Begin())
                {
                    _userRepository.Insert(new User
                    {
                        Name = outerUowPersonName,
                        Email = "someemail",
                        Surname = "somesurname"
                    });

                    using (IUnitOfWorkCompleteHandle innerUow = unitOfWorkManager.Begin(TransactionScopeOption.Suppress))
                    {
                        _userRepository.Insert(new User
                        {
                            Name = innerUowPersonName,
                            Email = "someemail",
                            Surname = "somesurname"
                        });

                        innerUow.Complete();
                    }

                    throw new ApplicationException("This exception is thown to rollback outer transaction!");
                }

                return;
            }).Message.ShouldBe("This exception is thown to rollback outer transaction!");

            UsingDbContext(context =>
            {
                context.Users.FirstOrDefault(n => n.Name == outerUowPersonName).ShouldBeNull();
                context.Users.FirstOrDefault(n => n.Name == innerUowPersonName).ShouldNotBeNull();
            });
        }
    }
}
