namespace ByteStore.Domain.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }

        // Foreign Keys
        public int ShoppingCartId { get; set; }
        public int ProductId { get; set; }

        // Navigation Properties
        public ShoppingCart ShoppingCart { get; set; }
        public Product Product { get; set; }
    }
}
