using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Transactions;

using Stove.Application.Services;
using Stove.Domain.Repositories;

namespace Stove.Domain.Uow
{
	internal class UnitOfWorkDefaultOptions : IUnitOfWorkDefaultOptions
	{
		private readonly List<DataFilterConfiguration> _filters;

		public UnitOfWorkDefaultOptions()
		{
			_filters = new List<DataFilterConfiguration>();
			IsTransactional = true;
			IsLazyLoadEnabled = true;
			Scope = TransactionScopeOption.Required;

			ConventionalUowSelectors = new List<Func<Type, bool>>
			{
				type => typeof(IRepository).GetTypeInfo().IsAssignableFrom(type) || typeof(IApplicationService).GetTypeInfo().IsAssignableFrom(type)
			};
		}

		public TransactionScopeOption Scope { get; set; }

		public bool IsTransactional { get; set; }

		public TimeSpan? Timeout { get; set; }

		public IsolationLevel? IsolationLevel { get; set; }

		public IReadOnlyList<DataFilterConfiguration> Filters => _filters;

		public List<Func<Type, bool>> ConventionalUowSelectors { get; }

		public bool IsLazyLoadEnabled { get; set; }

		public void RegisterFilter(string filterName, bool isEnabledByDefault)
		{
			if (_filters.Any(f => f.FilterName == filterName))
			{
				throw new StoveException("There is already a filter with name: " + filterName);
			}

			_filters.Add(new DataFilterConfiguration(filterName, isEnabledByDefault));
		}

		public void OverrideFilter(string filterName, bool isEnabledByDefault)
		{
			_filters.RemoveAll(f => f.FilterName == filterName);
			_filters.Add(new DataFilterConfiguration(filterName, isEnabledByDefault));
		}
	}
}
