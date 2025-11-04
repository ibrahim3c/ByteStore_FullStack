using ByteStore.Domain.Abstractions.Constants;
using ByteStore.Domain.Abstractions.Result;
using ByteStore.Domain.Entities;
using ByteStore.Domain.Repositories;
using BytStore.Application.DTOs.Auth;
using BytStore.Application.DTOs.Identity;
using BytStore.Application.IServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Net.Http;
using System.Security.Claims;
using MyResult = ByteStore.Domain.Abstractions.Result.Result;

namespace BytStore.Application.Services
{
    public sealed class AuthService:IAuthService 
    {
        private readonly UserManager<AppUser> userManager;
        private readonly IUnitOfWork uow;
        private readonly ITokenGenerator tokenGenerator;
        private readonly IEmailService emailService;

        public AuthService(UserManager<AppUser> userManager
            , IUnitOfWork uow
            , ITokenGenerator tokenGenerator
            ,IEmailService emailService)
        {
            this.userManager = userManager;
            this.uow = uow;
            this.tokenGenerator = tokenGenerator;
            this.emailService = emailService;
        }

        // JWT Token With Email Verification With RefreshToken
        public async Task<MyResult> RegisterAsync(UserRegisterDto userRegisterDto, string scheme, string host)
        {
            //  Create a new user
            var user = new AppUser
            {
                UserName = userRegisterDto.Fname + userRegisterDto.Lname,
                Email = userRegisterDto.Email,
                EmailConfirmed = false,
                PhoneNumber = userRegisterDto.PhoneNumber
            };

            var result = await userManager.CreateAsync(user, userRegisterDto.Password);
            if (!result.Succeeded)
                return MyResult.Failure(result.Errors.Select(e => e.Description).ToList());

            // add role to user
            await userManager.AddToRoleAsync(user, Roles.UserRole);

            //Create an Customer record linked to the user
            var customer = new Customer
            {
                DateOfBirth = userRegisterDto.BirthDate,
                FirstName = userRegisterDto.Fname,
                LastName = userRegisterDto.Lname,
                AppUserId = user.Id
            };

            await uow.CustomerRepository.AddAsync(customer);
            await uow.SaveChangesAsync();

            // send confirmation token to user 
            await SendConfirmationEmailAsync(user, scheme, host);

            return MyResult.Success(); 

        }

        public async Task<MyResult> VerifyEmailAsync(string userId, string code)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                return MyResult.Failure(["UserId and code are required"]);
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return MyResult.Failure(["User not found"]);
            }

            // Decode the token before using it
            var decodedCode = Uri.UnescapeDataString(code);

            var result = await userManager.ConfirmEmailAsync(user, decodedCode);
            if (result.Succeeded)
            {
                return MyResult.Success();
            }
            else
            {
                return MyResult.Failure(["Email confirmation failed"]);
            }
        }
        
        public async Task<AuthResult> LoginAsync(UserLoginDto UserDTO)
        {
            //var user = await userManager.FindByEmailAsync(UserDTO.Email);

            // to include the RefreshTokens 
            var user = await userManager.Users
                                    .Include(u => u.RefreshTokens)
                                    .FirstOrDefaultAsync(u => u.Email == UserDTO.Email);
            if (user == null)
                return new AuthResult
                {
                    IsSuccess = false,
                    Messages = ["Email or Password is incorrect"]
                };

            // verify if he confirmed
            if (!user.EmailConfirmed)
            {
                return new AuthResult
                {   
                    IsSuccess = false,
                    Messages = new List<string> { "Email needs to be Confirmed" }
                };
            }

            var result = await userManager.CheckPasswordAsync(user, UserDTO.Password);
            if (!result)
                return new AuthResult
                {
                    IsSuccess = false,
                    Messages = new List<string> { "Email or Password is incorrect" }
                };

            var token = await tokenGenerator.GenerateJwtTokenAsync(user);



            var authResult = new AuthResult()
            {
                IsSuccess = true,
                Token = token,
            };

            // check if user has already active refresh token 
            // so no need to give him new refresh token
            if (user.RefreshTokens.Any(r => r.IsActive))
            {
                // TODO: check this 
                var UserRefreshToken = user.RefreshTokens.FirstOrDefault(r => r.IsActive);
                authResult.RefreshToken = UserRefreshToken.Token;
                authResult.RefreshTokenExpiresOn = UserRefreshToken.ExpiresOn;
            }

            // if he does not
            // generate new refreshToken
            else
            {
                var refreshToken = tokenGenerator.GenereteRefreshToken();
                authResult.RefreshToken = refreshToken.Token;
                authResult.RefreshTokenExpiresOn = refreshToken.ExpiresOn;

                // then save it in db
                user.RefreshTokens.Add(refreshToken);
                await userManager.UpdateAsync(user);
            }

            return authResult;


        }
        private async Task SendConfirmationEmailAsync(AppUser user, string scheme, string host)
        {
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

            // Generate the URL =>https://localhost:7030/api/Accounts/verify-email?userId=dkl&code=ioerw
            //var callbackUrl = $"{scheme}://{host}/api/Accounts/verify-email?userId={user.Id}&code={Uri.EscapeDataString(code)}";
            //https://yourfrontend.com/verify-email?userId=123&code=XYZ
            var callbackUrl = $"http://localhost:4200/verify-email?userId={user.Id}&code={Uri.EscapeDataString(code)}";

            // it should be in the contrller not service
            //var callbackUrl = Url.Action(
            //    action: "VerifyEmail",          // اسم الميثود
            //    controller: "Accounts",         // اسم الكنترولر (من غير كلمة Controller)
            //    values: new { userId = user.Id, code },
            //    protocol: scheme,               // http أو https
            //    host: host                      // localhost:7030 مثلاً
            //);


            var emailBody = $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>Confirm Email</a>";

            // send Email
            await emailService.SendMailByBrevoAsync(user.Email!, "Email Confirmation", emailBody);

        }

        
        //  RefreshToken
        public async Task<AuthResult> RefreshTokenAsync(string refreshToken)
        {
            // ensure there is user has this refresh token
            var user = await userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(r => r.Token == refreshToken));
            if (user == null)
            {
                return new AuthResult
                {
                    // u can don't add false=> cuz it's the default value 
                    IsSuccess = false,
                    Messages = ["InValid Token"]
                };
            }
            // ensure this token is active
            var oldRefreshToken = user.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);
            if (!oldRefreshToken.IsActive)
                return new AuthResult
                {
                    IsSuccess = false,
                    Messages = ["InValid Token"]
                };
            // if all things well
            //revoke old refresh token
            oldRefreshToken.RevokedOn = DateTime.UtcNow;

            // generate new refresh token and add it to db
            var newRefreshToken = tokenGenerator.GenereteRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await userManager.UpdateAsync(user);

            // generate new JWT Token
            var jwtToken = await tokenGenerator.GenerateJwtTokenAsync(user);

            return new AuthResult
            {
                IsSuccess = true,
                Messages = ["Refresh Token Successfully"],
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiresOn = newRefreshToken.ExpiresOn,
                Token = jwtToken
            };

        }
        public async Task<bool> RevokeTokenAsync(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                return false;

            var user = await userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(r => r.Token == refreshToken));
            if (user == null)
                return false;

            var oldRefreshToken = user.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);
            if (!oldRefreshToken.IsActive)
                return false;

            oldRefreshToken.RevokedOn = DateTime.UtcNow;

            await userManager.UpdateAsync(user);

            return true;

        }


        //Forgot Password
        public async Task<Result<string>> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto, string scheme, string host)
        {
            var user = await userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
                return Result<string>.Failure(["Email is incorrect!"]);

            // generete token and  send it to user
            //await SendPasswordResetEmailAsync(user, scheme, host);
            await SendResetPasswordEmailAsync(user);


            return Result<string>.Success("Please go to your email and reset your password");


            // after that user click on link and go to frontend page that
            //1-capture userId, code
            //2-make form for user to reset new password
            // then user send data to reset password endpoint


        }
        private async Task SendPasswordResetEmailAsync(AppUser user, string scheme, string host)
        {
            // Generate the password reset token
            var code = await userManager.GeneratePasswordResetTokenAsync(user);

            // Construct the reset link
            var callbackUrl = $"{scheme}://{host}/api/Account/ResetPassword?userId={user.Id}&code={Uri.EscapeDataString(code)}";
            // Send email with the reset link
            //await mailingService.SendMailBySendGridAsync(user.Email, "Reset Your Password",
            //    $"Please reset your password by clicking this link: <a href='{callbackUrl}'>Reset Password</a>");
        }
        private async Task SendResetPasswordEmailAsync(AppUser user)
        {
            // Generate the password reset token
            var code = await userManager.GeneratePasswordResetTokenAsync(user);

            // Construct the reset link
            var callbackUrl = $"https://full-stack-website-react-asp-net-eight.vercel.app/reset-password?userId={user.Id}&code={Uri.EscapeDataString(code)}";
            // Send email with the reset link
            await emailService.SendMailByBrevoAsync(user.Email, "Reset Your Password",
                $"Please reset your password by clicking this link: <a href='{callbackUrl}'>Reset Password</a>");
        }
        public async Task<Result<string>> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            if (string.IsNullOrWhiteSpace(resetPasswordDto.UserId) || string.IsNullOrWhiteSpace(resetPasswordDto.code))
            {
                return Result<string>.Failure(["UserId and code are required"]);
            }

            var user = await userManager.FindByIdAsync(resetPasswordDto.UserId);
            if (user == null)
            {
                return Result<string>.Failure(["User not found"]);
            }

            // Decode the token before using it
            var decodedCode = Uri.UnescapeDataString(resetPasswordDto.code);

            var result = await userManager.ResetPasswordAsync(user, decodedCode, resetPasswordDto.NewPassword);
            if (result.Succeeded)
            {
                return Result<string>.Success("Password Reset successfully");
            }
            else
            {
                return Result<string>.Failure(["Error resetting password."]);
            }
        }

        /*
         
         Flow Between Frontend and Backend
            User Requests a Password Reset
                Frontend:
                    User enters their email in a "Forgot Password" form.
                    Frontend sends a POST request to ForgotPasswordAsync endpoint with the email.
                Backend (ForgotPasswordAsync)
                    Checks if the email exists.
                    Generates a password reset token.
                    Sends an email with a reset link (SendPasswordResetEmailAsync).
                    Returns a success message: "Please go to your email and reset your password".
            User Clicks the Reset Link in Email
                Frontend:
                    Extracts userId and code from the URL.
                    Displays a password reset form for the user to enter a new password.
            User Submits the New Password
                Frontend:
                    Sends a POST request to ResetPasswordAsync with userId, code, and newPassword.
                Backend (ResetPasswordAsync)
                    Validates input.
                    Finds the user.
                    Decodes the reset token.
                    Resets the password using userManager.ResetPasswordAsync().
                    Returns a success message: "Password Reset successfully".
         */

    }
}
