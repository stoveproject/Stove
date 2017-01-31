using System;

namespace Stove.Runtime
{
    public interface IAmbientScopeProvider<T>
        where T : class
    {
        T GetValue(string contextKey);

        IDisposable BeginScope(string contextKey, T value);
    }
}
