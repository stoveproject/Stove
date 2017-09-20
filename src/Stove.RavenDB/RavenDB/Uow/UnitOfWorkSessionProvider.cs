using Autofac.Extras.IocManager;

using Raven.Client.Documents.Session;

using Stove.Domain.Uow;

namespace Stove.RavenDB.Uow
{
	public class UnitOfWorkSessionProvider : ISessionProvider, ITransientDependency
	{
		private readonly ICurrentUnitOfWorkProvider _unitOfWorkProvider;

		public UnitOfWorkSessionProvider(ICurrentUnitOfWorkProvider unitOfWorkProvider)
		{
			_unitOfWorkProvider = unitOfWorkProvider;
		}

		public IDocumentSession Session => _unitOfWorkProvider.Current.GetSession();
	}
}
