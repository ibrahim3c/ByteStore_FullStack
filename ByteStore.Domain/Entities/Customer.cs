namespace ByteStore.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; set; } = new Guid();
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string AppUserId {  get; set; }
        public bool IsDeleted { get; set; }=false;
        public ICollection<Order> Orders { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<ProductReview> Reviews { get; set; }
        public AppUser AppUser { get;  set; }

        public string fullName => $"{FirstName} {LastName}";
    }
}
