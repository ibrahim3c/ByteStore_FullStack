using ByteStore.Domain.Abstractions.Constants;
using ByteStore.Domain.Abstractions.Result;
using ByteStore.PresentationLayer.Controllers;
using BytStore.Application.DTOs.Auth;
using BytStore.Application.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ByteStore.Presentation.Controllers
{
    public class AccountsController : BaseController
    {
        public AccountsController(IServiceManager serviceManager) : base(serviceManager)
        {
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            
            var result = await serviceManager.AuthService.RegisterAsync(dto, Request.Scheme, Request.Host.Value);
            return result.IsSuccess ? Ok() : BadRequest(result.Errors);
        }

        [HttpGet("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string userId, [FromQuery] string code)
        {
            var result = await serviceManager.AuthService.VerifyEmailAsync(userId, code);
            return result.IsSuccess ? Ok() : BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResult>> Login([FromBody] UserLoginDto dto)
        {
            var result = await serviceManager.AuthService.LoginAsync(dto);
            return result.IsSuccess ? Ok(result) : BadRequest(result.Messages);
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<AuthResult>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var result = await serviceManager.AuthService.RefreshTokenAsync(request.RefreshToken);
            if (!result.IsSuccess)
                return BadRequest(result.Messages);

            // set new refreshToken in response cookie
            setRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiresOn);

            return Ok(result);
       
        }

        [HttpPost("revoke-token")]
        public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequest request)
            {
            string token = request.RefreshToken ?? HttpContext.Request.Cookies[Keys.RefreshTokenKey];

            var result = await serviceManager.AuthService.RevokeTokenAsync(token);
            if (result)
                return Ok();
            return BadRequest("Invalid Token");
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var result = await serviceManager.AuthService.ForgotPasswordAsync(dto, Request.Scheme, Request.Host.Value);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            var result = await serviceManager.AuthService.ResetPasswordAsync(dto);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        private void setRefreshTokenInCookie(string refreshToken, DateTime refreshTokenExpiresOn)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshTokenExpiresOn.ToLocalTime()
            };

            HttpContext.Response.Cookies.Append(Keys.RefreshTokenKey, refreshToken, cookieOptions);
        }
    }

    // Helper class for JSON binding
    public class RefreshTokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
