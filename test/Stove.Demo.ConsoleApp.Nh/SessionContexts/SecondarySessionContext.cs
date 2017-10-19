using Stove.Demo.ConsoleApp.Nh.Entities;
using Stove.NHibernate.Enrichments;

namespace Stove.Demo.ConsoleApp.Nh.SessionContexts
{
    public class SecondarySessionContext : StoveSessionContext
    {
        public IStoveSessionSet<Category> Categories { get; set; }

    }
}
