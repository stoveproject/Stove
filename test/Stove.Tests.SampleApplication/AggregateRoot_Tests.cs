using System;

using Autofac.Extras.IocManager;

using Newtonsoft.Json.Linq;

using Shouldly;

using Stove.Commands;
using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.Events;
using Stove.Events.Bus;
using Stove.Events.Bus.Handlers;
using Stove.Json;
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

        [Fact]
        public void AggregateRoot_repository_with_TPrimaryKey()
        {
            Guid aggregateId = Guid.NewGuid();
            const string bundleName = "SampleBundle";

            string eventLog = string.Empty;
            string headerLog = string.Empty;

            The<IEventBus>().RegisterPublishingBehaviour((@event, headers) =>
            {
                eventLog = @event.ToJsonString(true, true);
                headerLog = headers.ToString();
            });

            The<IEventBus>().Register<ProductBundleCreated>((@event, headers) =>
            {
                @event.Name.ShouldBe(bundleName);
                @event.Id.ShouldBe(aggregateId);
                headers.GetAggregateId().ShouldBe(aggregateId.ToString());
            });

            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                The<IRepository<ProductBundle, Guid>>().Insert(ProductBundle.Create(aggregateId, bundleName));
                uow.Complete();
            }

            eventLog.ShouldNotBeEmpty();
            headerLog.ShouldNotBeEmpty();
        }

        [Fact]
        public void AggregateRoot_event_should_raise_but_if_correlationId_is_not_set_then_event_causationId_should_be_empty()
        {
            const string campaignName = "selam";

            The<IEventBus>().Register<CampaignCreatedEvent>((@event, headers) =>
            {
                @event.Name.ShouldBe(campaignName);
                headers[StoveConsts.Events.CausationId].ShouldBe(Guid.Empty.ToString());
            });

            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                The<IRepository<Campaign>>().Insert(new Campaign(campaignName));
                uow.Complete();
            }
        }

        [Fact]
        public void AggregateRoot_event_should_raise_and_correlationId_should_be_set()
        {
            const string campaignName = "selam";
            string correlationId = Guid.NewGuid().ToString();

            The<IEventBus>().Register<CampaignCreatedEvent>((@event, headers) =>
            {
                @event.Name.ShouldBe(campaignName);
                headers[StoveConsts.Events.CausationId].ShouldBe(correlationId);
            });

            using (The<IStoveCommandContextAccessor>().Use(correlationId))
            {
                using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
                {
                    The<IRepository<Campaign>>().Insert(new Campaign(campaignName));
                    uow.Complete();
                }
            }
        }

        [Fact]
        public void when_an_event_publishing_behaviour_is_defined_then_behavior_allow_to_edit_header_values()
        {
            const string campaignName = "selam";
            const string publishedBy = "From A Unit Test";
            string correlationId = Guid.NewGuid().ToString();

            The<IEventBus>().RegisterPublishingBehaviour((@event, headers) =>
            {
                headers["PublishedBy"] = publishedBy;
            });

            The<IEventBus>().Register<CampaignCreatedEvent>((@event, headers) =>
            {
                @event.Name.ShouldBe(campaignName);
                headers[StoveConsts.Events.CausationId].ShouldBe(correlationId);
                headers["PublishedBy"].ShouldBe(publishedBy);
            });

            using (The<IStoveCommandContextAccessor>().Use(correlationId))
            {
                using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
                {
                    The<IRepository<Campaign>>().Insert(new Campaign(campaignName));
                    uow.Complete();
                }
            }
        }

        [Fact]
        public void when_an_event_publishing_behaviour_is_defined_multiple_times_then_behavior_allow_to_edit_header_values()
        {
            const string campaignName = "selam";
            const string publishedBy = "From A Unit Test";
            const bool isMultiple = true;
            string correlationId = Guid.NewGuid().ToString();

            The<IEventBus>().RegisterPublishingBehaviour((@event, headers) =>
            {
                headers["PublishedBy"] = publishedBy;
            });

            The<IEventBus>().RegisterPublishingBehaviour((@event, headers) =>
            {
                headers["IsMultiple"] = isMultiple;
            });

            The<IEventBus>().Register<CampaignCreatedEvent>((@event, headers) =>
            {
                @event.Name.ShouldBe(campaignName);
                headers[StoveConsts.Events.CausationId].ShouldBe(correlationId);
                headers["PublishedBy"].ShouldBe(publishedBy);
                headers["IsMultiple"].ShouldBe(isMultiple);
            });

            using (The<IStoveCommandContextAccessor>().Use(correlationId))
            {
                using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
                {
                    The<IRepository<Campaign>>().Insert(new Campaign(campaignName));
                    uow.Complete();
                }
            }
        }
    }

    public class CampaignCreatedEventHandler : EventHandlerBase, IEventHandler<CampaignCreatedEvent>, ITransientDependency
    {
        public void Handle(CampaignCreatedEvent @event, Headers headers)
        {
            string name = @event.Name;

            string aggId = headers.GetAggregateId();
        }
    }
}
