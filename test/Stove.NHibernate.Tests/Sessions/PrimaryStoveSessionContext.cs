using Stove.NHibernate.Enrichments;
using Stove.NHibernate.Tests.Entities;

namespace Stove.NHibernate.Tests.Sessions
{
    public class PrimaryStoveSessionContext : StoveSessionContext
    {
        public IStoveSessionSet<Product> Products { get; set; }
    }
}
