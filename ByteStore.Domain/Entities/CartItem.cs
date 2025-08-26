namespace ByteStore.Domain.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        public int ShoppingCartId { get; set; }
        public int ProductId { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        // one product can be in many cart items (in different shopping carts)
        public Product Product { get; set; }
    }
}
