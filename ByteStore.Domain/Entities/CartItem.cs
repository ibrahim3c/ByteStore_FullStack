namespace ByteStore.Domain.Entities
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }  // snapshot
        public int Quantity { get; set; }
        public string ImageUrl { get; set; } = "";
        public string? BrandName { get; set; } // optional
    }

}
