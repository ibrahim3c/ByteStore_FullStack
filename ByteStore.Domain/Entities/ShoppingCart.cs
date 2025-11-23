namespace ByteStore.Domain.Entities
{
    public class ShoppingCart
    {
        public string Id { get; set; }
        public string PaymentIntentId { get; set; } = default!;  // stored in order
        public string ClientSecret { get; set; }      //  send it to front
        public List<CartItem> CartItems { get; set; } = new();
        //public int? DeliveryMethodId { get; set; }
        //public decimal ShippingPrice { get; set; }
    }
}
