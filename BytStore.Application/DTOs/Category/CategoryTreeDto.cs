

namespace ByteStore.Application.DTOs.Category
{
    public class CategoryTreeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }

        public List<CategoryTreeDto>? SubCategories { get; set; }
    }


}
