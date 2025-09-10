using ByteStore.Domain.Abstractions.Enums;
using ByteStore.Domain.Entities;

namespace BytStore.Application.DTOs.Order
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }

        public Guid CustomerId { get; set; }
        public int ShippingAddressId { get; set; }
        public int BillingAddressId { get; set; }

        public ICollection<GetOrderItemDto> OrderItems { get; set; }
    }
}
