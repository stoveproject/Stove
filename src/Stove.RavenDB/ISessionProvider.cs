using Raven.Client;

namespace Stove
{
    public interface ISessionProvider
    {
        IDocumentSession Session { get; }
    }
}
