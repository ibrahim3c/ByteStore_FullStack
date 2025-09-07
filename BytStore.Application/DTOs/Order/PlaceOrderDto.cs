namespace BytStore.Application.DTOs.Order
{
    public class PlaceOrderDto
    {
        public int CustomerId { get; set; }
        public int ShippingAddressId { get; set; }
        public int BillingAddressId { get; set; }
        public ICollection<OrderItemDto> OrderItems { get; set; }
    }
}
