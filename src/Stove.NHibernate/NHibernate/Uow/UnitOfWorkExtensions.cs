using System;

using NHibernate;

using Stove.Domain.Uow;

namespace Stove.NHibernate.Uow
{
    internal static class UnitOfWorkExtensions
    {
        public static ISession GetSession(this IActiveUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException(nameof(unitOfWork));
            }

            if (!(unitOfWork is NhUnitOfWork))
            {
                throw new ArgumentException("unitOfWork is not type of " + typeof(NhUnitOfWork).FullName, nameof(unitOfWork));
            }

            return (unitOfWork as NhUnitOfWork).Session;
        }
    }
}
