using System;

using NHibernate;

using Stove.Domain.Uow;
using Stove.NHibernate.Enrichments;

namespace Stove.NHibernate.Uow
{
    internal static class UnitOfWorkExtensions
    {
        public static ISession GetSession<TSessionContext>(this IActiveUnitOfWork unitOfWork) where TSessionContext : StoveSessionContext
        {
            if (unitOfWork == null) throw new ArgumentNullException(nameof(unitOfWork));

            if (!(unitOfWork is NhUnitOfWork)) throw new ArgumentException("unitOfWork is not type of " + typeof(NhUnitOfWork).FullName, nameof(unitOfWork));

            return ((NhUnitOfWork)unitOfWork).GetOrCreateSession<TSessionContext>();
        }
    }
}
