using ByteStore.Domain.Abstractions.Result;
using BytStore.Application.DTOs.Auth;
using MyResult = ByteStore.Domain.Abstractions.Result.Result;

namespace BytStore.Application.IServices
{
    public interface IAuthService
    {
        Task<MyResult> RegisterAsync(UserRegisterDto userRegisterDto, string scheme, string host);
        Task<MyResult> VerifyEmailAsync(string userId, string code);
        Task<AuthResult> LoginAsync(UserLoginDto userDto);
        Task<AuthResult> RefreshTokenAsync(string refreshToken);
        Task<bool> RevokeTokenAsync(string refreshToken);
        Task<Result<string>> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto, string scheme, string host);
        Task<Result<string>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    }
}
