using System;
using System.Collections.Generic;
using System.Linq;

using Shouldly;

using Stove.Dapper.Repositories;
using Stove.Dapper.Tests.CustomRepositories;
using Stove.Dapper.Tests.Entities;
using Stove.Domain.Repositories;
using Stove.Domain.Uow;
using Stove.Events.Bus;

using Xunit;

namespace Stove.Dapper.Tests
{
    public class StoveDapper_Tests : StoveDapperApplicationTestBase
    {
        private readonly IEventBus _eventBus;
        private readonly IMailRepository _mailCustomDapperRepository;
        private readonly IDapperRepository<Mail, Guid> _mailDapperRepository;
        private readonly IRepository<Mail, Guid> _mailRepository;
        private readonly IDapperRepository<Product> _productDapperRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public StoveDapper_Tests()
        {
            Building(builder => { }).Ok();

            _productDapperRepository = The<IDapperRepository<Product>>();
            _productRepository = The<IRepository<Product>>();
            _unitOfWorkManager = The<IUnitOfWorkManager>();
            _mailDapperRepository = The<IDapperRepository<Mail, Guid>>();
            _mailRepository = The<IRepository<Mail, Guid>>();
            _mailCustomDapperRepository = The<IMailRepository>();
            _eventBus = The<IEventBus>();
        }

        [Fact]
        public void Dapper_Repository_Tests()
        {
            using (IUnitOfWorkCompleteHandle uow = _unitOfWorkManager.Begin())
            {
                //---Insert operation should work and tenant, creation audit properties must be set---------------------
                _productDapperRepository.Insert(new Product("TShirt"));
                Product insertedProduct = _productDapperRepository.GetAll(x => x.Name == "TShirt").FirstOrDefault();

                Guid mailId = _mailRepository.InsertAndGetId(new Mail("Hi There !"));
                _unitOfWorkManager.Current.SaveChanges();
                Mail mail = _mailDapperRepository.Get(mailId);
                mail.ShouldNotBeNull();
                mail.CreatorUserId.ShouldNotBeNull();
                mail.CreatorUserId.ShouldBe(StoveSession.UserId);

                Mail mailFromCustomRepository = _mailCustomDapperRepository.GetMailById(mailId);
                mailFromCustomRepository.ShouldNotBeNull();

                insertedProduct.ShouldNotBeNull();
                insertedProduct.CreationTime.ShouldNotBeNull();
                insertedProduct.CreatorUserId.ShouldNotBeNull();
                insertedProduct.CreatorUserId.ShouldBe(StoveSession.UserId);

                //----Update operation should work and Modification Audits should be set---------------------------
                _productDapperRepository.Insert(new Product("TShirt"));
                Product productToUpdate = _productDapperRepository.GetAll(x => x.Name == "TShirt").FirstOrDefault();
                productToUpdate.Name = "Pants";
                _productDapperRepository.Update(productToUpdate);

                productToUpdate.ShouldNotBeNull();

                productToUpdate.CreationTime.ShouldNotBeNull();
                productToUpdate.LastModifierUserId.ShouldBe(StoveSession.UserId);
                productToUpdate.CreatorUserId.ShouldBe(StoveSession.UserId);

                //---Get method should return single-------------------------------------------------------------------
                _productDapperRepository.Insert(new Product("TShirt"));
                Action getAction = () => _productDapperRepository.Single(x => x.Name == "TShirt");

                getAction.ShouldThrow<InvalidOperationException>("Sequence contains more than one element");

                //----Select * from syntax should work---------------------------------
                IEnumerable<Product> products = _productDapperRepository.Query("select * from Products");

                products.Count().ShouldBeGreaterThan(0);

                //------------Ef and Dapper should work under same transaction---------------------
                Product productFromEf = _productRepository.FirstOrDefault(x => x.Name == "TShirt");
                Product productFromDapper = _productDapperRepository.Single(productFromEf.Id);

                productFromDapper.Name.ShouldBe(productFromEf.Name);

                //------Soft Delete should work for Dapper--------------
                _productDapperRepository.Insert(new Product("SoftDeletableProduct"));

                Product toSoftDeleteProduct = _productDapperRepository.Single(x => x.Name == "SoftDeletableProduct");

                _productDapperRepository.Delete(toSoftDeleteProduct);

                toSoftDeleteProduct.IsDeleted.ShouldBe(true);
                toSoftDeleteProduct.DeleterUserId.ShouldBe(StoveSession.UserId);

                Product softDeletedProduct = _productRepository.FirstOrDefault(x => x.Name == "SoftDeletableProduct");
                softDeletedProduct.ShouldBeNull();

                Product softDeletedProductFromDapper = _productDapperRepository.FirstOrDefault(x => x.Name == "SoftDeletableProduct");
                softDeletedProductFromDapper.ShouldBeNull();

                using (_unitOfWorkManager.Current.DisableFilter(StoveDataFilters.SoftDelete))
                {
                    Product softDeletedProductWhenFilterDisabled = _productRepository.FirstOrDefault(x => x.Name == "SoftDeletableProduct");
                    softDeletedProductWhenFilterDisabled.ShouldNotBeNull();

                    Product softDeletedProductFromDapperWhenFilterDisabled = _productDapperRepository.Single(x => x.Name == "SoftDeletableProduct");
                    softDeletedProductFromDapperWhenFilterDisabled.ShouldNotBeNull();
                }

                using (StoveSession.Use(266))
                {
                    _productDapperRepository.Insert(new Product("InsertedProductWith266Id"));
                    Product InsertedProductWith266Id = _productDapperRepository.GetAll(x => x.Name == "InsertedProductWith266Id").FirstOrDefault();

                    InsertedProductWith266Id.ShouldNotBeNull();
                    InsertedProductWith266Id.CreationTime.ShouldNotBeNull();
                    InsertedProductWith266Id.CreatorUserId.ShouldNotBeNull();
                    InsertedProductWith266Id.CreatorUserId.ShouldBe(StoveSession.UserId);
                }

                _productDapperRepository.Insert(new Product("InsertedProductAfterSpecifiedUserId"));
                Product insertedProductAfterSpecifiedUserId = _productDapperRepository.GetAll(x => x.Name == "InsertedProductAfterSpecifiedUserId").FirstOrDefault();

                insertedProductAfterSpecifiedUserId.ShouldNotBeNull();
                insertedProductAfterSpecifiedUserId.CreationTime.ShouldNotBeNull();
                insertedProductAfterSpecifiedUserId.CreatorUserId.ShouldNotBeNull();
                insertedProductAfterSpecifiedUserId.CreatorUserId.ShouldBe(StoveSession.UserId);

                uow.Complete();
            }
        }
    }

    public class ProductCreatedEvent : Event
    {
    }
}
