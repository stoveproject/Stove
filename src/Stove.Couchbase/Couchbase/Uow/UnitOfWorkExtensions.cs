using System;

using Couchbase.Linq;

using Stove.Domain.Uow;

namespace Stove.Couchbase.Uow
{
    public static class UnitOfWorkExtensions
    {
        public static IBucketContext GetSession(this IActiveUnitOfWork unitOfWork)
        {
            if (unitOfWork == null) throw new ArgumentNullException(nameof(unitOfWork));

            if (!(unitOfWork is CouchbaseUnitOfWork)) throw new ArgumentException("unitOfWork is not type of " + typeof(CouchbaseUnitOfWork).FullName, nameof(unitOfWork));

            return ((CouchbaseUnitOfWork)unitOfWork).Session;
        }
    }
}
