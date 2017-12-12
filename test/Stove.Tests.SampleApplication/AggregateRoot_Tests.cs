using System;

using Autofac.Extras.IocManager;

using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.Events.Bus;
using Stove.Events.Bus.Handlers;
using Stove.Tests.SampleApplication.Domain.Entities;
using Stove.Tests.SampleApplication.Domain.Events;

using Xunit;

namespace Stove.Tests.SampleApplication
{
    public class AggregateRoot_Tests : SampleApplicationTestBase
    {
        public AggregateRoot_Tests()
        {
            Building(builder => { }).Ok();
        }

        [Fact]
        public void AggreateRoot_event_should_raise_when_Added()
        {
            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                The<IRepository<Campaign>>().Insert(new Campaign("selam"));
                uow.Complete();
            }
        }
    }

    public class CampaignCreatedEventHandler : EventHandlerBase, IEventHandler<CampaignCreatedEvent>, ITransientDependency
    {
        public void Handle(CampaignCreatedEvent eventData)
        {
            string name = eventData.Name;
        }
    }
}
