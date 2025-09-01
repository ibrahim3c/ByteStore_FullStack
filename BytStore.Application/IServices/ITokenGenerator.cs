using ByteStore.Domain.Entities;

namespace BytStore.Application.IServices
{
    public interface ITokenGenerator
    {
        Task<string> GenerateJwtTokenAsync(AppUser appUser);
        RefreshToken GenereteRefreshToken();
    }
}
