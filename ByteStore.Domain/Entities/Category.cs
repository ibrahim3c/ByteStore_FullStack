namespace ByteStore.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int? ParentCategoryId { get; set; } // For sub-categories (e.g., Laptops -> Gaming Laptops)

        public Category? ParentCategory { get; set; }
        public ICollection<Category> SubCategories { get; set; } // One-to-many self-reference
        public ICollection<Product> Products { get; set; } 
    }
}
