using System.ComponentModel.DataAnnotations;

namespace BytStore.Application.DTOs.Identity
{
    public class UpdateUserDto
    {
        public string UserId { get; set; }
        public string UserName { get; set; }

        public string? Address { get; set; } = default!;

        [Phone]
        public string? PhoneNumber { get; set; }
    }
}
