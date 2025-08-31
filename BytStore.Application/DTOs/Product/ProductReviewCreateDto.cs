namespace BytStore.Application.DTOs.Product
{
    public class ProductReviewCreateDto
    {
        public int CustomerId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
