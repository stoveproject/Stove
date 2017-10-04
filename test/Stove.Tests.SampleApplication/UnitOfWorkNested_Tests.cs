using System;
using System.Threading.Tasks;
using System.Transactions;

using NSubstitute;

using Shouldly;

using Stove.Domain.Uow;

using Xunit;

namespace Stove.Tests.SampleApplication
{
    public class UnitOfWorkNested_Tests : SampleApplicationTestBase
    {
        public UnitOfWorkNested_Tests()
        {
            Building(builder => { }).Ok();
            _unitOfWorkManager = The<IUnitOfWorkManager>();
        }

        private readonly IUnitOfWorkManager _unitOfWorkManager;

        [Fact]
        public void Should_Copy_Filters_To_Nested_Uow()
        {
            using (IUnitOfWorkCompleteHandle outerUow = _unitOfWorkManager.Begin())
            {
                _unitOfWorkManager.Current.EnableFilter(StoveDataFilters.SoftDelete);

                _unitOfWorkManager.Current.IsFilterEnabled(StoveDataFilters.SoftDelete).ShouldBe(true);

                using (_unitOfWorkManager.Current.DisableFilter(StoveDataFilters.SoftDelete))
                {
                    using (IUnitOfWorkCompleteHandle nestedUow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                    {
                        //Because nested transaction copies outer uow's filters.
                        _unitOfWorkManager.Current.IsFilterEnabled(StoveDataFilters.SoftDelete).ShouldBe(false);

                        nestedUow.Complete();
                    }

                    _unitOfWorkManager.Current.IsFilterEnabled(StoveDataFilters.SoftDelete).ShouldBe(false);
                }

                _unitOfWorkManager.Current.IsFilterEnabled(StoveDataFilters.SoftDelete).ShouldBe(true);

                outerUow.Complete();
            }

            _unitOfWorkManager.Current.ShouldBeNull();
        }

        [Fact]
        public async Task uow_multithread_current_shouldnotbenull()
        {
            Parallel.For(0, 1000, i =>
            {
                var provider = The<ICurrentUnitOfWorkProvider>();

                var uow = Substitute.For<IUnitOfWork>();
                uow.Id.Returns(Guid.NewGuid().ToString("N"));

                provider.Current = uow;

                provider.Current.ShouldNotBeNull();

                provider.Current = null;

                provider.Current.ShouldBeNull();
            });
        }
    }
}
