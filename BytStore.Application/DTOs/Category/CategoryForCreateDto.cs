using Microsoft.AspNetCore.Http;

namespace ByteStore.Application.DTOs.Category
{
    public class CategoryForCreateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile Image { get; set; }

    }
}
