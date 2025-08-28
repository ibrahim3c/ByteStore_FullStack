namespace BytStore.Application.DTOs.Product
{
    public class ProductListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string BrandName { get; set; }
        public string CategoryName { get; set; }
    }
}
