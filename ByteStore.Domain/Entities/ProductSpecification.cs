namespace ByteStore.Domain.Entities
{
    public class ProductSpecification
    {
        public int Id { get; set; }
        public string Key { get; set; } // e.g., "Screen Size"
        public string Value { get; set; } // e.g., "15.6 inches"

        // Foreign Key
        public int ProductId { get; set; }
        // Navigation Property
        public Product Product { get; set; }
    }
}
