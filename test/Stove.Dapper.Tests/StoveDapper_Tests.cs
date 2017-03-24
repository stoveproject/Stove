using System;
using System.Collections.Generic;
using System.Linq;

using Shouldly;

using Stove.Dapper.Repositories;
using Stove.Dapper.Tests.Entities;
using Stove.Domain.Repositories;
using Stove.Domain.Uow;

using Xunit;

namespace Stove.Dapper.Tests
{
    public class StoveDapper_Tests : StoveDapperApplicationTestBase
    {
        private readonly IDapperRepository<Product> _productDapperRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<Mail, Guid> _mailRepository;
        private readonly IDapperRepository<Mail, Guid> _mailDapperRepository;

        public StoveDapper_Tests()
        {
            Building(builder => { }).Ok();

            _productDapperRepository = The<IDapperRepository<Product>>();
            _productRepository = The<IRepository<Product>>();
            _unitOfWorkManager = The<IUnitOfWorkManager>();
            _mailDapperRepository = The<IDapperRepository<Mail, Guid>>();
            _mailRepository = The<IRepository<Mail, Guid>>();
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
                mail.CreatorUserId.ShouldBe(TestStoveSession.UserId);

                insertedProduct.ShouldNotBeNull();
                insertedProduct.CreationTime.ShouldNotBeNull();
                insertedProduct.CreatorUserId.ShouldNotBeNull();
                insertedProduct.CreatorUserId.ShouldBe(TestStoveSession.UserId);

                //----Update operation should work and Modification Audits should be set---------------------------
                _productDapperRepository.Insert(new Product("TShirt"));
                Product productToUpdate = _productDapperRepository.GetAll(x => x.Name == "TShirt").FirstOrDefault();
                productToUpdate.Name = "Pants";
                _productDapperRepository.Update(productToUpdate);

                productToUpdate.ShouldNotBeNull();

                productToUpdate.CreationTime.ShouldNotBeNull();
                productToUpdate.LastModifierUserId.ShouldBe(TestStoveSession.UserId);
                productToUpdate.CreatorUserId.ShouldBe(TestStoveSession.UserId);

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
                toSoftDeleteProduct.DeleterUserId.ShouldBe(TestStoveSession.UserId);

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

                using (TestStoveSession.Use(266))
                {
                    _productDapperRepository.Insert(new Product("InsertedProductWith266Id"));
                    Product InsertedProductWith266Id = _productDapperRepository.GetAll(x => x.Name == "InsertedProductWith266Id").FirstOrDefault();

                    InsertedProductWith266Id.ShouldNotBeNull();
                    InsertedProductWith266Id.CreationTime.ShouldNotBeNull();
                    InsertedProductWith266Id.CreatorUserId.ShouldNotBeNull();
                    InsertedProductWith266Id.CreatorUserId.ShouldBe(TestStoveSession.UserId);
                }

                _productDapperRepository.Insert(new Product("InsertedProductAfterSpecifiedUserId"));
                Product InsertedProductAfterSpecifiedUserId = _productDapperRepository.GetAll(x => x.Name == "InsertedProductAfterSpecifiedUserId").FirstOrDefault();

                InsertedProductAfterSpecifiedUserId.ShouldNotBeNull();
                InsertedProductAfterSpecifiedUserId.CreationTime.ShouldNotBeNull();
                InsertedProductAfterSpecifiedUserId.CreatorUserId.ShouldNotBeNull();
                InsertedProductAfterSpecifiedUserId.CreatorUserId.ShouldBe(TestStoveSession.UserId);

                uow.Complete();
            }
        }
    }
}
