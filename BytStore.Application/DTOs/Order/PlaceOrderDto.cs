namespace BytStore.Application.DTOs.Order
{
    public class PlaceOrderDto
    {
        public Guid CustomerId { get; set; }
        public int ShippingAddressId { get; set; }
        public int BillingAddressId { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; }
    }
}
