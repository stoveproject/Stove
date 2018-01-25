using Stove.Events.Bus;

namespace Stove.NHibernate.Tests.Entities.Events
{
    public class ProductNameFixed : Event
    {
        public readonly string Name;

        public ProductNameFixed(string name)
        {
            Name = name;
        }
    }
}
