namespace ByteStore.Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; } // Snapshot of the price when ordered

        // Foreign Keys
        public Guid OrderId { get; set; }
        public int ProductId { get; set; }

        // Navigation Properties
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
