using System;

using Raven.Client.Documents.Session;

using Stove.Domain.Uow;

namespace Stove.RavenDB.Uow
{
	internal static class UnitOfWorkExtensions
	{
		public static IDocumentSession GetSession(this IActiveUnitOfWork unitOfWork)
		{
			if (unitOfWork == null)
			{
				throw new ArgumentNullException(nameof(unitOfWork));
			}

			if (!(unitOfWork is RavenDBUnitOfWork))
			{
				throw new ArgumentException("unitOfWork is not type of " + typeof(RavenDBUnitOfWork).FullName, nameof(unitOfWork));
			}

			return (unitOfWork as RavenDBUnitOfWork).Session;
		}
	}
}
