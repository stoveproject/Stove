using Stove.Events.Bus;

namespace Stove.EntityFrameworkCore.Tests.Domain_Product.Events
{
    public class VariantValueAdded : Event
    {
        public int Id { get; }

        public string Value { get; }

        public VariantValueAdded(int id, string value)
        {
            Id = id;
            Value = value;
        }
    }
}
