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
            
            EventBus = NullEventBus.Instance;
        }

        public IEventBus EventBus { get; set; }

        public void Publish<T>(T @event) where T : IEvent
        {
            CheckCurrentUow();

            _unitOfWorkManager.Current.Completed += (sender, args) => { EventBus.Publish(@event); };
        }

        private void CheckCurrentUow()
        {
            if (_unitOfWorkManager.Current == null)
            {
                throw new StoveException($"{nameof(IUnitOfWorkCompletedEventHelper.Publish)} method should be called in a UOW.");
            }
        }
    }
}
