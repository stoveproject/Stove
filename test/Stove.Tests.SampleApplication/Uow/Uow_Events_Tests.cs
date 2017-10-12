using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

using Autofac;
using Autofac.Extras.DynamicProxy;
using Autofac.Extras.IocManager;

using Castle.DynamicProxy;

using Shouldly;

using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.Events.Bus;
using Stove.Events.Bus.Handlers;
using Stove.Tests.SampleApplication.Domain.Entities;

using Xunit;

namespace Stove.Tests.SampleApplication.Uow
{
    public class Uow_Events_Tests : SampleApplicationTestBase
    {
        public Uow_Events_Tests()
        {
            Building(builder =>
            {
                builder.RegisterServices(r =>
                {
                    r.UseBuilder(cb =>
                    {
                        cb.RegisterType<Provider>()
                          .AsSelf()
                          .WithPropertyInjection()
                          .EnableClassInterceptors()
                          .InterceptedBy(typeof(SomeKindOfInterceptor));
                    });
                });
            }).Ok();

            _unitOfWorkManager = The<IUnitOfWorkManager>();
            _userRepository = The<IRepository<User>>();
        }

        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<User> _userRepository;

        [Fact]
        public void Should_Trigger_Completed_When_Uow_Succeed()
        {
            var completeCount = 0;
            var disposeCount = 0;

            using (IUnitOfWorkCompleteHandle uow = _unitOfWorkManager.Begin())
            {
                _userRepository.Insert(new User { Name = "Oğuzhan", Email = "osoykan@gmail", Surname = "Soykan" });

                _unitOfWorkManager.Current.Completed += (sender, args) => { completeCount++; };

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

        [Fact]
        public void Should_Trigger_events_with_proxied_objects()
        {
            var completeCount = 0;
            var disposeCount = 0;

            Parallel.For(0, 500, i =>
            {
                var uowManager = The<IUnitOfWorkManager>();
                using (IUnitOfWorkCompleteHandle uow = uowManager.Begin())
                {
                    The<IRepository<User>>().Insert(new User { Name = "Oğuzhan", Email = "osoykan@gmail", Surname = "Soykan" });

                    uowManager.Current.Completed += (sender, args) =>
                    {
                        completeCount++;
                    };

                    uowManager.Current.Disposed += (sender, args) =>
                    {
                        var provider = The<Provider>();
                        provider.ShouldNotBeNull();
                        uowManager.Current.ShouldBe(null);
                        disposeCount++;
                    };

                    The<IRepository<User>>().FirstOrDefault(x=>x.Name == "Oğuzhan").ShouldNotBeNull();

                    uow.Complete();

                    The<IEventBus>().Trigger(new SomeUowEvent());
                }
            });             
        }
    }

    public class SomeUowEvent : EventData
    {
    }

    public class SomeUowEventHandler : IEventHandler<SomeUowEvent>, ITransientDependency
    {
        private readonly Provider _provider;

        public SomeUowEventHandler(Provider provider)
        {
            _provider = provider;
        }

        public void HandleEvent(SomeUowEvent eventData)
        {
            var a = 1;
            _provider.ShouldNotBeNull();
            _provider.SomeStuff();
        }
    }

    public class SomeKindOfInterceptor : IInterceptor, ITransientDependency
    {
        public void Intercept(IInvocation invocation)
        {
            var a = 1;
            invocation.Proceed();
        }
    }

    public class Provider
    {
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public Provider(IRepository<User> userRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _userRepository = userRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _userRepository.ShouldNotBeNull();
        }

        public virtual void SomeStuff()
        {
            using (IUnitOfWorkCompleteHandle uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                using (IUnitOfWorkCompleteHandle uow1 = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                {
                    _userRepository.FirstOrDefault(1).ShouldNotBeNull();

                    using (IUnitOfWorkCompleteHandle uow2 = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                    {
                        _userRepository.FirstOrDefault(1).ShouldNotBeNull();

                        using (IUnitOfWorkCompleteHandle uow3 = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                        {
                            _userRepository.FirstOrDefault(1).ShouldNotBeNull();

                            using (IUnitOfWorkCompleteHandle uow4 = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                            {
                                _userRepository.FirstOrDefault(1).ShouldNotBeNull();

                                uow4.Complete();
                            }

                            uow3.Complete();
                        }

                        uow2.Complete();
                    }

                    uow1.Complete();
                }

                _userRepository.FirstOrDefault(1).ShouldNotBeNull();

                uow.Complete();
            }
        }
    }
}
