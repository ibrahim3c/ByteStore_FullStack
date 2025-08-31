namespace BytStore.Application.DTOs.Product
{
    public class ProductReviewUpdateDto
    {
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
    }
}
