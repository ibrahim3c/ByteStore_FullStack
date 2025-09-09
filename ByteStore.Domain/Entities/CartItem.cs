namespace ByteStore.Domain.Entities
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }  // snapshot
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string? BrandName { get; set; } = string.Empty; // optional
        public string CategoryName { get; set; } = string.Empty; // i don't know 
    }

}
