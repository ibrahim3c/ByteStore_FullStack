namespace BytStore.Application.DTOs.Customer
{
    public class CustomerDto:CustomerUpdateDto
    {
        public int Id { get; set; }
        public string Email { get; set; }

    }
}
