using ByteStore.Domain.Abstractions.Enums;

namespace BytStore.Application.DTOs.Customer
{
    public class AddressDto
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public bool IsPrimary { get; set; }
        public string AddressType { get; set; } // "Shipping" or "Billing"
        public string CustomerName {  get; set; }
    }
}
