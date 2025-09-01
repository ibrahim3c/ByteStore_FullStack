namespace BytStore.Application.DTOs.Customer
{
    public class CustomerUpdateDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
    }
}
