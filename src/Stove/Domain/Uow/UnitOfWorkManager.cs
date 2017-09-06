using System.Linq;
using System.Transactions;

using Autofac.Extras.IocManager;

namespace Stove.Domain.Uow
{
	/// <summary>
	///     Unit of work manager.
	/// </summary>
	internal class UnitOfWorkManager : IUnitOfWorkManager, ITransientDependency
	{
		private readonly ICurrentUnitOfWorkProvider _currentUnitOfWorkProvider;
		private readonly IUnitOfWorkDefaultOptions _defaultOptions;
		private readonly IScopeResolver _scopeResolver;
		private IScopeResolver _childScope;

		public UnitOfWorkManager(
			IScopeResolver scopedResolver,
			ICurrentUnitOfWorkProvider currentUnitOfWorkProvider,
			IUnitOfWorkDefaultOptions defaultOptions)
		{
			_scopeResolver = scopedResolver;
			_currentUnitOfWorkProvider = currentUnitOfWorkProvider;
			_defaultOptions = defaultOptions;
		}

		public IActiveUnitOfWork Current => _currentUnitOfWorkProvider.Current;

		public IUnitOfWorkCompleteHandle Begin()
		{
			return Begin(new UnitOfWorkOptions());
		}

		public IUnitOfWorkCompleteHandle Begin(TransactionScopeOption scope)
		{
			return Begin(new UnitOfWorkOptions { Scope = scope });
		}

		public IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options)
		{
			_childScope = _scopeResolver.BeginScope();

			options.FillDefaultsForNonProvidedOptions(_defaultOptions);

			IUnitOfWork outerUow = _currentUnitOfWorkProvider.Current;

			if (options.Scope == TransactionScopeOption.Required && outerUow != null)
			{
				return new InnerUnitOfWorkCompleteHandle();
			}

			var uow = _childScope.Resolve<IUnitOfWork>();

			uow.Completed += (sender, args) => { _currentUnitOfWorkProvider.Current = null; };

			uow.Failed += (sender, args) => { _currentUnitOfWorkProvider.Current = null; };

			uow.Disposed += (sender, args) => { _childScope.Dispose(); };

			//Inherit filters from outer UOW
			if (outerUow != null)
			{
				options.FillOuterUowFiltersForNonProvidedOptions(outerUow.Filters.ToList());
			}

			uow.Begin(options);

			_currentUnitOfWorkProvider.Current = uow;

			return uow;
		}
	}
}
