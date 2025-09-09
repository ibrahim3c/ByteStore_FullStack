namespace ByteStore.Domain.Entities
{
    public class ShoppingCart
    {
        public string CustomerId { get; set; }
        public List<CartItem> CartItems { get; set; } = new();
    }
}
