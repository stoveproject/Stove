using System.Linq;

using Shouldly;

using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.Tests.SampleApplication.Domain.Entities;

using Xunit;

namespace Stove.Tests.SampleApplication.Uow
{
    public class Uow_Events_Tests : SampleApplicationTestBase
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<User> _userRepository;

        public Uow_Events_Tests()
        {
            Building(builder => {}).Ok();

            _unitOfWorkManager = The<IUnitOfWorkManager>();
            _userRepository = The<IRepository<User>>();
        }

        [Fact]
        public void Should_Trigger_Completed_When_Uow_Succeed()
        {
            var completeCount = 0;
            var disposeCount = 0;

            using (IUnitOfWorkCompleteHandle uow = _unitOfWorkManager.Begin())
            {
                _userRepository.Insert(new User { Name = "Oğuzhan", Email = "osoykan@gmail", Surname = "Soykan" });

                _unitOfWorkManager.Current.Completed += (sender, args) =>
                {
                    completeCount++;
                };

                _unitOfWorkManager.Current.Disposed += (sender, args) =>
                {
                    _unitOfWorkManager.Current.ShouldBe(null);
                    completeCount.ShouldBe(1);
                    disposeCount++;
                };

                uow.Complete();
            }

            UsingDbContext(context => context.Users.Any(p => p.Name == "Oğuzhan").ShouldBe(true));

            completeCount.ShouldBe(1);
            disposeCount.ShouldBe(1);
        }
    }
}
