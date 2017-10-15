using Couchbase.Linq;

namespace Stove.Couchbase.Couchbase
{
    public interface ISessionProvider
    {
        IBucketContext Session { get; }
    }
}
