namespace ByteStore.Domain.Entities
{
    public class ProductImage
    {
        public int id { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPrimary { get; set; } // Indicates if this is the primary image for the product
        public string Title { get; set; } // Optional title or alt text for the image

        public int ProductId { get; set; }
        public Product Product { get; set; }

    }
}
