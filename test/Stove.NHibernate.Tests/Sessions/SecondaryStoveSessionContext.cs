using Stove.NHibernate.Enrichments;
using Stove.NHibernate.Tests.Entities;

namespace Stove.NHibernate.Tests.Sessions
{
    public class SecondaryStoveSessionContext : StoveSessionContext
    {
        public IStoveSessionSet<Category> Categories { get; set; }
    }
}
