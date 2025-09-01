using Microsoft.AspNetCore.Identity;

namespace ByteStore.Domain.Entities
{
    public class AppUser:IdentityUser
    {
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
