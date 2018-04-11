using System;
using System.Threading;
using System.Threading.Tasks;

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

        public static async Task SilentlyAsync(Func<Task> callback, Func<Exception, Task> onException, Func<Task> finallyCallback = null, CancellationToken cancellationToken = default)
        {
            try
            {
                await callback();
            }
            catch (Exception ex)
            {
                await onException(ex);
            }
            finally
            {
                if (finallyCallback != null)
                {
                    await finallyCallback();
                }
            }
        }
    }
}
