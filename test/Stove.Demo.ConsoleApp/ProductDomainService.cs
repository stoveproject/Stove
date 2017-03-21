using System;
using System.Collections.Generic;
using System.Linq;

using Stove.Dapper.Repositories;
using Stove.Demo.ConsoleApp.Entities;
using Stove.Domain.Services;
using Stove.Domain.Uow;
using Stove.Runtime.Session;

namespace Stove.Demo.ConsoleApp
{
    public class ProductDomainService : DomainService
    {
        private readonly IDapperRepository<Mail, Guid> _mailDapperRepository;
        private readonly IDapperRepository<Product> _productDapperRepository;

        public ProductDomainService(IDapperRepository<Product> productDapperRepository, IDapperRepository<Mail, Guid> mailDapperRepository)
        {
            _productDapperRepository = productDapperRepository;
            _mailDapperRepository = mailDapperRepository;
        }

        public IStoveSession StoveSession { get; set; }

        public void DoSomeStuff()
        {
            using (IUnitOfWorkCompleteHandle uow = UnitOfWorkManager.Begin())
            {
                using (StoveSession.Use(266))
                {
                    _productDapperRepository.Insert(new Product("TShirt"));
                    int gomlekId = _productDapperRepository.InsertAndGetId(new Product("Gomlek"));

                    Product firstProduct = _productDapperRepository.Get(1);
                    IEnumerable<Product> products = _productDapperRepository.GetAll();

                    firstProduct.Name = "Something";

                    _productDapperRepository.Update(firstProduct);

                    _mailDapperRepository.Insert(new Mail("New Product Added"));
                    Guid mailId = _mailDapperRepository.InsertAndGetId(new Mail("Second Product Added"));

                    IEnumerable<Mail> mails = _mailDapperRepository.GetAll();

                    Mail firstMail = mails.First();

                    firstMail.Subject = "Sorry wrong email!";

                    _mailDapperRepository.Update(firstMail);
                }

                uow.Complete();
            }
        }
    }
}
