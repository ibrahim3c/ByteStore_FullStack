namespace ByteStore.Domain.Entities
{
    public class ShoppingCart
    {
        public int CustomerId { get; set; }
        public List<CartItem> CartItems { get; set; } = new();
    }
}
