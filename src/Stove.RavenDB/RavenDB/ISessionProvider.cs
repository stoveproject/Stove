using Raven.Client.Documents.Session;

namespace Stove.RavenDB
{
    public interface ISessionProvider
    {
        IDocumentSession Session { get; }
    }
}
