using ByteStore.Domain.Abstractions.Enums;

namespace ByteStore.Domain.Entities
{
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public bool IsPrimary { get; set; }
        public AddressType AddressType { get; set; } // "Shipping" or "Billing"

        // Foreign Key
        public Guid CustomerId { get; set; }
        // Navigation Property
        public Customer Customer { get; set; }
    }
}
