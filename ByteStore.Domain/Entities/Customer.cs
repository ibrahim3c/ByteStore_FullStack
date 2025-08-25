namespace ByteStore.Domain.Entities
{
    public class Customer
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        // Navigation Properties
        public ICollection<Order> Orders { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ShoppingCart ShoppingCart { get; set; } // One-to-One relationship
        public ICollection<ProductReview> Reviews { get; set; }
    }
}
