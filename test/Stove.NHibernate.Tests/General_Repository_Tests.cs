using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

using Autofac;
using Autofac.Extras.DynamicProxy;
using Autofac.Extras.IocManager;

using Castle.DynamicProxy;

using NHibernate;

using Shouldly;

using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.Events.Bus;
using Stove.Events.Bus.Handlers;
using Stove.NHibernate.Repositories;
using Stove.NHibernate.Tests.Entities;
using Stove.NHibernate.Tests.Entities.Events;
using Stove.NHibernate.Tests.Sessions;

using Xunit;

using IInterceptor = Castle.DynamicProxy.IInterceptor;

namespace Stove.NHibernate.Tests
{
    public class General_Repository_Tests : StoveNHibernateTestBase
    {
        public General_Repository_Tests()
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
            StoveSession.UserId = 1;
            UsingSession<PrimaryStoveSessionContext>(session => { session.Save(new Product("TShirt")); });
        }

        [Fact]
        public void GetAll_should_work()
        {
            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                var repository = The<IRepository<Product>>();
                repository.GetAll().Count().ShouldBe(1);
                repository.GetAllList().Count.ShouldBe(1);

                uow.Complete();
            }
        }

        [Fact]
        public void Insert_should_work()
        {
            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                The<IRepository<Product>>().Insert(new Product("Pants"));

                Product product = UsingSession<PrimaryStoveSessionContext, Product>(session => session.Query<Product>().FirstOrDefault(p => p.Name == "Pants"));
                product.ShouldNotBe(null);
                product.IsTransient().ShouldBe(false);
                product.Name.ShouldBe("Pants");

                uow.Complete();
            }
        }

        [Fact]
        public void Insert_should_work_on_multi_database()
        {
            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                The<IRepository<Product>>().Insert(new Product("Pants"));
                The<IRepository<Category>>().Insert(new Category("Bread"));

                Product product = UsingSession<PrimaryStoveSessionContext, Product>(session => session.Query<Product>().FirstOrDefault(p => p.Name == "Pants"));
                product.ShouldNotBe(null);
                product.IsTransient().ShouldBe(false);
                product.Name.ShouldBe("Pants");

                Category category = The<IRepository<Category>>().FirstOrDefault(x => x.Name == "Bread");
                category.ShouldNotBe(null);
                category.IsTransient().ShouldBe(false);
                category.Name.ShouldBe("Bread");

                uow.Complete();
            }
        }

        [Fact]
        public void QueryOver_should_work_on_generic_repsitories()
        {
            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                ISession session = The<IRepository<Product>>().GetSession();

                IList<Product> products = session.QueryOver<Product>().List();

                products.Count.ShouldBeGreaterThan(0);

                uow.Complete();
            }
        }

        [Fact]
        public async Task Should_rollback_when_CancellationToken_cancel_is_requested()
        {
            var ts = new CancellationTokenSource();
            var eventTriggerCount = 0;
            try
            {
                using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
                {
                    The<IEventBus>().Register<ProductNameFixed>(
                        (@event, headers) =>
                        {
                            @event.Name.ShouldBe("Pants");
                            ts.Cancel(true);
                            eventTriggerCount++;
                        });

                    Product product = The<IRepository<Product>>().Single(p => p.Name == "TShirt");

                    product.FixName("Pants");

                    await uow.CompleteAsync(ts.Token);
                }
            }
            catch (Exception exception)
            {
                //Handle
            }

            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                The<IRepository<Product>>().FirstOrDefault(x => x.Name == "Pants").ShouldBeNull();

                await uow.CompleteAsync();
            }

            eventTriggerCount.ShouldBe(1);
        }

        [Fact]
        public void Should_Trigger_Event_On_Delete()
        {
            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                var triggerCount = 0;

                The<IEventBus>().Register<ProductDeletedEvent>(
                    (@event, headers) =>
                    {
                        @event.Name.ShouldBe("TShirt");
                        triggerCount++;
                    });

                Product product = The<IRepository<Product>>().Single(p => p.Name == "TShirt");
                product.Delete();

                The<IRepository<Product>>().FirstOrDefault(p => p.Name == "TShirt").ShouldBe(null);

                uow.Complete();

                triggerCount.ShouldBe(1);
            }
        }

        [Fact]
        public void Should_Trigger_Event_On_Insert()
        {
            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                var triggerCount = 0;

                The<IEventBus>().Register<ProductCreatedEvent>(
                    (@event, headers) =>
                    {
                        @event.Name.ShouldBe("Kazak");
                        triggerCount++;
                    });

                The<IRepository<Product>>().Insert(new Product("Kazak"));

                uow.Complete();

                triggerCount.ShouldBe(1);
            }
        }

        [Fact]
        public void Should_Trigger_Event_On_Update()
        {
            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                var triggerCount = 0;

                The<IEventBus>().Register<ProductNameFixed>(
                    (@event, headers) =>
                    {
                        @event.Name.ShouldBe("Kazak");
                        triggerCount++;
                    });

                Product product = The<IRepository<Product>>().Single(p => p.Name == "TShirt");
                product.FixName("Kazak");
                The<IRepository<Product>>().Update(product);

                uow.Complete();

                triggerCount.ShouldBe(1);
            }
        }

        [Fact(Skip = "Flaky! Find a better way.")]
        public void Should_Trigger_events_with_proxied_objects_in_multithread()
        {
            var completeCount = 0;
            var disposeCount = 0;

            var tasks = new List<Task>();
            Parallel.For(0, 20, i =>
            {
                tasks.Add(Task.Run(() =>
                {
                    var uowManager = The<IUnitOfWorkManager>();
                    using (IUnitOfWorkCompleteHandle uow = uowManager.Begin())
                    {
                        The<IRepository<Product>>().Insert(new Product("Oguzhan"));
                        The<IRepository<Category>>().Insert(new Category("Selam"));

                        uowManager.Current.Completed += (sender, args) => { completeCount++; };

                        uowManager.Current.Disposed += (sender, args) =>
                        {
                            var provider = The<Provider>();
                            provider.ShouldNotBeNull();
                            uowManager.Current.ShouldBe(null);
                            disposeCount++;
                        };

                        The<IRepository<Product>>().FirstOrDefault(x => x.Name == "Oguzhan").ShouldNotBeNull();
                        The<IRepository<Category>>().FirstOrDefault(x => x.Name == "Selam").ShouldNotBeNull();

                        uow.Complete();

                        The<IEventBus>().Publish(new SomeUowEvent(), new Headers());
                    }
                }));
            });

            Task.WaitAll(tasks.ToArray());

            disposeCount.ShouldBeGreaterThanOrEqualTo(1);
            completeCount.ShouldBeGreaterThanOrEqualTo(1);
        }

        [Fact]
        public void Update_With_Action_Test()
        {
            using (IUnitOfWorkCompleteHandle uow = The<IUnitOfWorkManager>().Begin())
            {
                Product productBefore = UsingSession<PrimaryStoveSessionContext, Product>(session => session.Query<Product>().Single(p => p.Name == "TShirt"));

                Product updatedUser = The<IRepository<Product>>().Update(productBefore.Id, user => user.Name = "Polo");
                updatedUser.Id.ShouldBe(productBefore.Id);
                updatedUser.Name.ShouldBe("Polo");

                The<IUnitOfWorkManager>().Current.SaveChanges();

                Product productAfter = UsingSession<PrimaryStoveSessionContext, Product>(session => session.Get<Product>(productBefore.Id));
                productAfter.Name.ShouldBe("Polo");

                uow.Complete();
            }
        }
    }

    public class SomeUowEvent : Event
    {
    }

    public class SomeUowEventHandler : EventHandlerBase, IEventHandler<SomeUowEvent>, ITransientDependency
    {
        private readonly Provider _provider;

        public SomeUowEventHandler(Provider provider)
        {
            _provider = provider;
        }

        public void Handle(SomeUowEvent @event, Headers headers)
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
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public Provider(
            IRepository<Product> productRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<Category> categoryRepository)
        {
            _productRepository = productRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _categoryRepository = categoryRepository;
            _productRepository.ShouldNotBeNull();
        }

        public virtual void SomeStuff()
        {
            using (IUnitOfWorkCompleteHandle uow = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                using (IUnitOfWorkCompleteHandle uow1 = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                {
                    _productRepository.FirstOrDefault(1).ShouldNotBeNull();
                    _categoryRepository.FirstOrDefault(1).ShouldNotBeNull();

                    using (IUnitOfWorkCompleteHandle uow2 = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                    {
                        _productRepository.FirstOrDefault(1).ShouldNotBeNull();
                        _categoryRepository.FirstOrDefault(1).ShouldNotBeNull();

                        using (IUnitOfWorkCompleteHandle uow3 = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                        {
                            _productRepository.FirstOrDefault(1).ShouldNotBeNull();
                            _categoryRepository.FirstOrDefault(1).ShouldNotBeNull();

                            using (IUnitOfWorkCompleteHandle uow4 = _unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                            {
                                _productRepository.FirstOrDefault(1).ShouldNotBeNull();
                                _categoryRepository.FirstOrDefault(1).ShouldNotBeNull();

                                uow4.Complete();
                            }

                            uow3.Complete();
                        }

                        uow2.Complete();
                    }

                    uow1.Complete();
                }

                _productRepository.FirstOrDefault(1).ShouldNotBeNull();
                _categoryRepository.FirstOrDefault(1).ShouldNotBeNull();

                uow.Complete();
            }
        }
    }
}
