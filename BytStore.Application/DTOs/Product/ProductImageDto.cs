namespace BytStore.Application.DTOs.Product
{
    public class ProductImageDto
    {
        public string ImageUrl { get; set; }
        public bool IsPrimary { get; set; } // Indicates if this is the primary image for the product

    }
}
