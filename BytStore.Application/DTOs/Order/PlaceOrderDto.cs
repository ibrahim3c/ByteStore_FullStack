namespace BytStore.Application.DTOs.Order
{
    public class PlaceOrderDto
    {
        public string CartId { get; set; }
        public string CustomerId { get; set; }
        public int ShippingAddressId { get; set; }
        public int BillingAddressId { get; set; }
    }
}
