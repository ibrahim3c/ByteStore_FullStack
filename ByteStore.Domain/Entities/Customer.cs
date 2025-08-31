namespace ByteStore.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ShoppingCart ShoppingCart { get; set; }
        public ICollection<ProductReview> Reviews { get; set; }

        // TODO: add relation with IdentityUser
        public string fullName => $"{FirstName} {LastName}";
    }
}
