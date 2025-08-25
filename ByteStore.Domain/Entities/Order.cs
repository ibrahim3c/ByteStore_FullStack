namespace ByteStore.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } // e.g., "Pending", "Processing", "Shipped", "Delivered", "Cancelled"

        // Foreign Keys
        public int UserId { get; set; }
        public int ShippingAddressId { get; set; }
        public int BillingAddressId { get; set; }

        // Navigation Properties
        public Customer User { get; set; }
        public Address ShippingAddress { get; set; }
        public Address BillingAddress { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
