using System.Threading;
using System.Threading.Tasks;

namespace Stove.Events.Bus.Entities
{
    /// <summary>
    ///     Null-object implementation of <see cref="IEntityChangeEventHelper" />.
    /// </summary>
    public class NullEntityChangeEventHelper : IEntityChangeEventHelper
    {
        private NullEntityChangeEventHelper()
        {
        }

        /// <summary>
        ///     Gets single instance of <see cref="NullEventBus" /> class.
        /// </summary>
        public static NullEntityChangeEventHelper Instance { get; } = new NullEntityChangeEventHelper();

        public void PublishEvents(EntityChangeReport changeReport)
        {
        }

        public Task PublishEventsAsync(EntityChangeReport changeReport, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(0);
        }
    }
}
