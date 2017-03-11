using System.Transactions;

using Shouldly;

using Stove.Domain.Uow;

using Xunit;

namespace Stove.Tests.SampleApplication
{
    public class UnitOfWorkNested_Tests : SampleApplicationTestBase
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UnitOfWorkNested_Tests()
        {
            Building(builder => { }).Ok();
            _unitOfWorkManager = The<IUnitOfWorkManager>();
        }

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
        }
    }
}
