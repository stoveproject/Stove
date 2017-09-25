using Raven.Client;
using Raven.Client.Documents.Session;

namespace Stove
{
    public interface ISessionProvider
    {
        IDocumentSession Session { get; }
    }
}
