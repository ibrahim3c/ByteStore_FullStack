namespace BytStore.Application.DTOs.Auth
{
    public class ForgotPasswordDto
    {
        public string BaseUrl { get; set; } = default!;
        public string Email { get; set; } = default!;
    }
}
