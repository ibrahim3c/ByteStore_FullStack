namespace ByteStore.Domain.Entities
{
    public class ShoppingCart
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public int CustomerId { get; set; }
        // Navigation Properties
        public Customer Customer { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
