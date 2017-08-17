using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using Stove.Dapper.Repositories;
using Stove.Dapper.TableValueParameters;
using Stove.Demo.ConsoleApp.Dto;
using Stove.Demo.ConsoleApp.Entities;
using Stove.Domain.Services;
using Stove.Domain.Uow;
using Stove.Runtime.Session;

namespace Stove.Demo.ConsoleApp
{
    public class PriceDomainService : DomainService
    {
        private readonly IDapperRepository<StorefrontProductPrice> _storefrontStorefrontProductPriceDapperRepository;

        public PriceDomainService(IDapperRepository<StorefrontProductPrice> storefrontStorefrontProductPriceDapperRepository)
        {
            _storefrontStorefrontProductPriceDapperRepository = storefrontStorefrontProductPriceDapperRepository;
        }

        public IStoveSession StoveSession { get; set; }

        public void DoSomeStuff()
        {
            IEnumerable<StorefrontProductCurrencyDto> storefrontProductPriceDto = new List<StorefrontProductCurrencyDto>
            {
                new StorefrontProductCurrencyDto { CurrencyId = 1, ProductId = 10001, StorefrontId = 1 }
            };

            using (IUnitOfWorkCompleteHandle uow = UnitOfWorkManager.Begin())
            {
                IEnumerable<StorefrontProductPriceDto> test = _storefrontStorefrontProductPriceDapperRepository.Query<StorefrontProductPriceDto>("dbo.[GetProductPrice]", new TableValueParameter("@ProductPrice", "ProductPrice", storefrontProductPriceDto), CommandType.StoredProcedure).ToList();

                Console.WriteLine(test.Count());


                uow.Complete();
            }
        }
    }
}
