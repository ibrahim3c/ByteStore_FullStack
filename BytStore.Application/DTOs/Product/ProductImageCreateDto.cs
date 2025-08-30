using Microsoft.AspNetCore.Http;

namespace BytStore.Application.DTOs.Product
{
    public class ProductImageCreateDto
    {
        public IFormFile Image { get; set; }
        public bool IsPrimary { get; set; } = false;
    }
}
