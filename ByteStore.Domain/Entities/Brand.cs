namespace ByteStore.Domain.Entities
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        //public string? LogoUrl { get; set; }

        // Navigation Property
        public ICollection<Product> Products { get; set; } // One Brand has many Products
    }
}
