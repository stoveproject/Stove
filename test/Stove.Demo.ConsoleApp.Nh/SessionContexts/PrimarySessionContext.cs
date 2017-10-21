using Stove.Demo.ConsoleApp.Nh.Entities;
using Stove.NHibernate.Enrichments;

namespace Stove.Demo.ConsoleApp.Nh.SessionContexts
{
    public class PrimarySessionContext : StoveSessionContext
    {
        public IStoveSessionSet<Product> Categories { get; set; }
    }
}
