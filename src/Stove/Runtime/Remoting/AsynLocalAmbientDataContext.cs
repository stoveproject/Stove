using System.Collections.Concurrent;
using System.Threading;

using Autofac.Extras.IocManager;

namespace Stove.Runtime.Remoting
{
	public class AsyncLocalAmbientDataContext : IAmbientDataContext, ISingletonDependency
	{
		private static readonly ConcurrentDictionary<string, AsyncLocal<object>> asyncLocalDictionary = new ConcurrentDictionary<string, AsyncLocal<object>>();

		public void SetData(string key, object value)
		{
			AsyncLocal<object> asyncLocal = asyncLocalDictionary.GetOrAdd(key, (k) => new AsyncLocal<object>());
			asyncLocal.Value = value;
		}

		public object GetData(string key)
		{
			AsyncLocal<object> asyncLocal = asyncLocalDictionary.GetOrAdd(key, (k) => new AsyncLocal<object>());
			return asyncLocal.Value;
		}
	}
}
