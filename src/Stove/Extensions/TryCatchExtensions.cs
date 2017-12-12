using System;

namespace Stove.Extensions
{
    public static class Try
    {
        public static void Silently(Action callback, Action<Exception> onException, Action finallyCallback = null)
        {
            try
            {
                callback();
            }
            catch (Exception exception)
            {
                onException(exception);
            }
            finally
            {
                finallyCallback?.Invoke();
            }
        }
    }
}
