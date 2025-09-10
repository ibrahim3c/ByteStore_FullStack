namespace BytStore.Application.DTOs.Customer
{
    public class CustomerDto:CustomerUpdateDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
    }
}
