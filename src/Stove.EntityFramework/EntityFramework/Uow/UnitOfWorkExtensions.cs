using System;
using System.Data.Entity;

using Stove.Domain.Uow;

namespace Stove.EntityFramework.Uow
{
    public static class UnitOfWorkExtensions
    {
        public static TDbContext GetDbContext<TDbContext>(this IActiveUnitOfWork unitOfWork)
            where TDbContext : DbContext
        {
            if (unitOfWork == null) throw new ArgumentNullException(nameof(unitOfWork));

            if (!(unitOfWork is EfUnitOfWork)) throw new ArgumentException("unitOfWork is not type of " + typeof(EfUnitOfWork).FullName, nameof(unitOfWork));

            return ((EfUnitOfWork)unitOfWork).GetOrCreateDbContext<TDbContext>();
        }
    }
}
