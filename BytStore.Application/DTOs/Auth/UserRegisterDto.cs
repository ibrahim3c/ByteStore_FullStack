namespace BytStore.Application.DTOs.Auth
{
    public class UserRegisterDto
    {
        public string Fname { get; set; } = default!;
        public string Lname { get; set; } = default!;
        public int CountryId { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
        public DateTime BirthDate { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string ConfirmPassword { get; set; } = default!;
    }
}
