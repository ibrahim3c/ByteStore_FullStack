namespace ByteStore.Domain.Entities
{
    public class ProductReview
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; } // e.g., 1 to 5
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public int ProductId { get; set; }
        public int UserId { get; set; }
        public Product Product { get; set; }
        public Customer Customer { get; set; }
    }
}
