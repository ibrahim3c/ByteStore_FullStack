using ByteStore.Domain.Abstractions.Enums;
using BytStore.Application.DTOs.Customer;

namespace BytStore.Application.DTOs.Order
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }

        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int ShippingAddressId { get; set; }
        public OrderAddressDto ShippingAddress { get; set; }
        public int BillingAddressId { get; set; }
        public OrderAddressDto BillingAddress { get; set; }

        public ICollection<GetOrderItemDto> OrderItems { get; set; }
    }
}
