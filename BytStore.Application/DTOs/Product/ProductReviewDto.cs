namespace BytStore.Application.DTOs.Product
{
    public class ProductReviewDto
    {
        public string Commenet { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CustomerName { get; set; }
    }
}
