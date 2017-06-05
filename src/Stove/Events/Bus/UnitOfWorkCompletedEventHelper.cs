using System.Threading.Tasks;

using Autofac.Extras.IocManager;

using Stove.Domain.Uow;

namespace Stove.Events.Bus
{
    public class UnitOfWorkCompletedEventHelper : IUnitOfWorkCompletedEventHelper, ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public UnitOfWorkCompletedEventHelper(IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
        }

        public IEventBus EventBus { get; set; }

        public void Trigger<T>(T @event) where T : IEventData
        {
            CheckCurrentUow();

            _unitOfWorkManager.Current.Completed += (sender, args) => { EventBus.Trigger(@event); };
        }

        public Task TriggerAsync<T>(T @event) where T : IEventData
        {
            CheckCurrentUow();

            _unitOfWorkManager.Current.Completed += async (sender, args) => { await EventBus.TriggerAsync(@event); };
            return Task.FromResult(0);
        }

        private void CheckCurrentUow()
        {
            if (_unitOfWorkManager.Current == null)
            {
                throw new StoveException($"{nameof(IUnitOfWorkCompletedEventHelper.Trigger)} method should be called in a UOW.");
            }
        }
    }
}
