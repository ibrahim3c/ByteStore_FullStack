using ByteStore.Domain.Abstractions.Constants;
using ByteStore.Domain.Abstractions.Result;
using ByteStore.PresentationLayer.Controllers;
using BytStore.Application.DTOs.Auth;
using BytStore.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace ByteStore.Presentation.Controllers
{
    [EnableRateLimiting("token")]
    public class AccountsController : BaseController
    {
        public AccountsController(IServiceManager serviceManager) : base(serviceManager)
        {
        }


        [Authorize]
        [HttpGet("me")]
        public async Task<IActionResult> GetCurrentUser()
        {
            var user = await serviceManager.UserService.GetCurrentUserAsync(User);
            return Ok(user);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
        {
            var result = await serviceManager.AuthService.RegisterAsync(dto, Request.Scheme, Request.Host.Value);
            return result.IsSuccess ? Ok(new { message = "Please verify your email through the verification email we have just sent." }) : BadRequest(result.Errors);
        }

        [HttpGet("verify-email")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyEmail([FromQuery] string userId, [FromQuery] string code)
        {
            var result = await serviceManager.AuthService.VerifyEmailAsync(userId, code);
            return result.IsSuccess ? Ok() : BadRequest(result.Errors);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResult>> Login([FromBody] UserLoginDto dto)
        {
            var result = await serviceManager.AuthService.LoginAsync(dto);

            if (result.IsSuccess)
            {
            //set refresh token in response cookie
            setRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiresOn);
             return   Ok(result); 
            }
            return BadRequest(result.Messages);
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthResult>> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            string token = request.RefreshToken ?? HttpContext.Request.Cookies[Keys.RefreshTokenKey];
            var result = await serviceManager.AuthService.RefreshTokenAsync(token);
            if (!result.IsSuccess)
                return BadRequest(result.Messages);

            // set new refreshToken in response cookie
            setRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiresOn);

            return Ok(result);
       
        }

        [HttpPost("revoke-token")]
        [Authorize]
        public async Task<IActionResult> RevokeToken([FromBody] RefreshTokenRequest request)
            {
            string token = request.RefreshToken ?? HttpContext.Request.Cookies[Keys.RefreshTokenKey];

            var result = await serviceManager.AuthService.RevokeTokenAsync(token);
            if (result)
                return Ok();
            return BadRequest("Invalid Token");
        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var result = await serviceManager.AuthService.ForgotPasswordAsync(dto, Request.Scheme, Request.Host.Value);
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Errors);
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
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
