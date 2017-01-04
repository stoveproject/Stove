using System;

using Stove.Domain.Values;

namespace Stove.Tests.Domain.Values
{
    public class Address : ValueObject<Address>
    {
        public Address(
            Guid cityId,
            string street,
            int number)
        {
            CityId = cityId;
            Street = street;
            Number = number;
        }

        public Guid CityId { get; }

        public string Street { get; }

        public int Number { get; }
    }

    public class Address2 : ValueObject<Address2>
    {
        public Address2(
            Guid? cityId,
            string street,
            int number)
        {
            CityId = cityId;
            Street = street;
            Number = number;
        }

        public Guid? CityId { get; }

        public string Street { get; }

        public int Number { get; }
    }
}
