namespace BytStore.Application.DTOs.Product
{
    public class ProductDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public List<ProductImageDto> Images { get; set; } = new();
        public List<ProductReviewDto> Reviews { get; set; } = new();

        public string BrandName { get; set; }
        public string CategoryName { get; set; }
    }
}
