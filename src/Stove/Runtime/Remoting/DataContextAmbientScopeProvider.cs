using System;
using System.Collections.Concurrent;

using JetBrains.Annotations;

using Stove.Collections.Extensions;
using Stove.Domain.Uow;

namespace Stove.Runtime.Remoting
{
    /// <summary>
    ///     CallContext implementation of <see cref="ICurrentUnitOfWorkProvider" />.
    ///     This is the default implementation.
    /// </summary>
    public class DataContextAmbientScopeProvider<T> : IAmbientScopeProvider<T>
        where T : class
    {
        private static readonly ConcurrentDictionary<string, ScopeItem> scopeDictionary = new ConcurrentDictionary<string, ScopeItem>();

        private readonly IAmbientDataContext _dataContext;

        public DataContextAmbientScopeProvider([NotNull] IAmbientDataContext dataContext)
        {
            Check.NotNull(dataContext, nameof(dataContext));

            _dataContext = dataContext;
        }

        public T GetValue(string contextKey)
        {
            return GetCurrentItem(contextKey)?.Value;
        }

        public IDisposable BeginScope(string contextKey, T value)
        {
            var item = new ScopeItem(value, GetCurrentItem(contextKey));

            if (!scopeDictionary.TryAdd(item.Id, item))
            {
                throw new StoveException("Can not set unit of work! ScopeDictionary.TryAdd returns false!");
            }

            _dataContext.SetData(contextKey, item.Id);

            return new DisposeAction(() =>
            {
                scopeDictionary.TryRemove(item.Id, out item);

                if (item.Outer == null)
                {
                    _dataContext.SetData(contextKey, null);
                    return;
                }

                _dataContext.SetData(contextKey, item.Outer.Id);
            });
        }

        private ScopeItem GetCurrentItem(string contextKey)
        {
            return _dataContext.GetData(contextKey) is string objKey ? scopeDictionary.GetOrDefault(objKey) : null;
        }

        private class ScopeItem
        {
            public ScopeItem(T value, ScopeItem outer = null)
            {
                Id = Guid.NewGuid().ToString();

                Value = value;
                Outer = outer;
            }

            public string Id { get; }

            public ScopeItem Outer { get; }

            public T Value { get; }
        }
    }
}
