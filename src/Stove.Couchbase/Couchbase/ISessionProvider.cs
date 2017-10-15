using Couchbase.Linq;

namespace Stove.Couchbase
{
    public interface ISessionProvider
    {
        IBucketContext Session { get; }
    }
}
