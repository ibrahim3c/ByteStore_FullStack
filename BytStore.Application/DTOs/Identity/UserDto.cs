using System.ComponentModel.DataAnnotations;

namespace BytStore.Application.DTOs.Identity
{
    public class UserDto
    {
        //[Required]
        //[StringLength(255, MinimumLength = 5, ErrorMessage = "Address must be between 5 and 255 characters.")]
        //public string Address { get; set; } = default!;
        [Required]
        public string UserName { get; set; } = default;
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = default!;

        [Required]
        [Phone]
        //[RegularExpression(@"^(010|011|012|015)\d{8}$", ErrorMessage = "Invalid Egyptian phone number.")]
        public string PhoneNumber { get; set; } = default!;

        public bool IsActive { get; set; } = true; // Default to active
    }
}
