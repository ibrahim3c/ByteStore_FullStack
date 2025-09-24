using ByteStore.Domain.Abstractions.Constants;
using ByteStore.Domain.Entities;
using ByteStore.Domain.Repositories;
using BytStore.Application.DTOs.Auth;
using BytStore.Application.IServices;
using BytStore.Application.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace ByteStore.Application.UnitTests.Services
{
    public class AuthServiceTests
    {
        // Mocks for all dependencies of AuthService
        private readonly Mock<UserManager<AppUser>> _mockUserManager;
        private readonly Mock<IUnitOfWork> _mockUow;
        private readonly Mock<ITokenGenerator> _mockTokenGenerator;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;

        // The instance of the service we are testing
        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            // UserManager requires a user store mock to be constructed
            var userStoreMock = new Mock<IUserStore<AppUser>>();
            _mockUserManager = new Mock<UserManager<AppUser>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);

            // Standard mocks for interfaces
            _mockUow = new Mock<IUnitOfWork>();
            _mockTokenGenerator = new Mock<ITokenGenerator>();
            _mockEmailService = new Mock<IEmailService>();
            _mockCustomerRepository = new Mock<ICustomerRepository>();

            // When the Unit of Work is asked for the CustomerRepository, return our mock
            _mockUow.Setup(u => u.CustomerRepository).Returns(_mockCustomerRepository.Object);

            // Create the service instance with all the mocked dependencies
            _authService = new AuthService(
                _mockUserManager.Object,
                _mockUow.Object,
                _mockTokenGenerator.Object,
                _mockEmailService.Object);
        }

        #region RegisterAsync Tests

        [Fact]
        public async Task RegisterAsync_ShouldReturnSuccess_WhenUserCreationSucceeds()
        {
            // Arrange
            var userRegisterDto = new UserRegisterDto { Email = "test@example.com", Password = "Password123!", Fname = "Test", Lname = "User" };

            // Setup UserManager mocks to simulate success
            _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<AppUser>(), userRegisterDto.Password))
                            .ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(um => um.AddToRoleAsync(It.IsAny<AppUser>(), Roles.UserRole))
                            .ReturnsAsync(IdentityResult.Success);
            _mockUserManager.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<AppUser>()))
                            .ReturnsAsync("dummy_confirmation_token");

            // Act
            var result = await _authService.RegisterAsync(userRegisterDto, "https", "localhost");

            // Assert
            result.IsSuccess.Should().BeTrue();

            // Verify that the correct methods were called on our mocks
            _mockUserManager.Verify(um => um.CreateAsync(It.IsAny<AppUser>(), userRegisterDto.Password), Times.Once);
            _mockCustomerRepository.Verify(cr => cr.AddAsync(It.IsAny<Customer>()), Times.Once);
            _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Once);
            // u should add value for optional parameter cuz moq does not understand it
            _mockEmailService.Verify(es => es.SendMailByBrevoAsync(userRegisterDto.Email, "Email Confirmation", It.IsAny<string>(), null), Times.Once);
        }

        [Fact]
        public async Task RegisterAsync_ShouldReturnFailure_WhenUserCreationFails()
        {
            // Arrange
            var userRegisterDto = new UserRegisterDto { Email = "test@example.com", Password = "Password123!" };
            var identityErrors = new List<IdentityError> { new IdentityError { Description = "Password is too weak." } };

            _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<AppUser>(), userRegisterDto.Password))
                            .ReturnsAsync(IdentityResult.Failed(identityErrors.ToArray()));

            // Act
            var result = await _authService.RegisterAsync(userRegisterDto, "https", "localhost");

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain("Password is too weak.");

            // Ensure no further actions (like saving customer) were taken
            _mockCustomerRepository.Verify(cr => cr.AddAsync(It.IsAny<Customer>()), Times.Never);
            _mockUow.Verify(uow => uow.SaveChangesAsync(), Times.Never);
        }

        #endregion

        #region VerifyEmailAsync Tests

        [Fact]
        public async Task VerifyEmailAsync_ShouldReturnSuccess_WhenUserAndCodeAreValid()
        {
            // Arrange
            var userId = "valid_user_id";
            var code = "valid_code";
            var user = new AppUser { Id = userId };

            _mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            _mockUserManager.Setup(um => um.ConfirmEmailAsync(user, code)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authService.VerifyEmailAsync(userId, code);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task VerifyEmailAsync_ShouldReturnFailure_WhenUserNotFound()
        {
            // Arrange
            var userId = "invalid_user_id";
            var code = "valid_code";

            _mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync((AppUser)null);

            // Act
            var result = await _authService.VerifyEmailAsync(userId, code);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain("User not found");
        }

        [Fact]
        public async Task VerifyEmailAsync_ShouldReturnFailure_WhenConfirmationFails()
        {
            // Arrange
            var userId = "valid_user_id";
            var code = "invalid_code";
            var user = new AppUser { Id = userId };

            _mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            _mockUserManager.Setup(um => um.ConfirmEmailAsync(user, code))
                            .ReturnsAsync(IdentityResult.Failed());

            // Act
            var result = await _authService.VerifyEmailAsync(userId, code);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Errors.Should().Contain("Email confirmation failed");
        }

        #endregion

        #region LoginAsync Tests

        [Fact]
        public async Task LoginAsync_ShouldReturnSuccessWithNewRefreshToken_WhenCredentialsAreValidAndNoActiveTokenExists()
        {
            // Arrange
            var userLoginDto = new UserLoginDto { Email = "test@example.com", Password = "Password123!" };
            var user = new AppUser { Email = userLoginDto.Email, EmailConfirmed = true };
            var newRefreshToken = new RefreshToken { Token = "new_refresh_token", ExpiresOn = DateTime.UtcNow.AddDays(7) };

            // Mocking the user retrieval (note: LoginAsync uses a direct EF Core query, so we mock the Users property)
            var users = new List<AppUser> { user }.AsQueryable();
            var asyncUsers = new TestAsyncEnumerable<AppUser>(users);

            _mockUserManager.Setup(um => um.Users).Returns(asyncUsers);
            _mockUserManager.Setup(um => um.CheckPasswordAsync(user, userLoginDto.Password)).ReturnsAsync(true);
            _mockTokenGenerator.Setup(tg => tg.GenerateJwtTokenAsync(user)).ReturnsAsync("new_jwt_token");
            _mockTokenGenerator.Setup(tg => tg.GenereteRefreshToken()).Returns(newRefreshToken);
            _mockUserManager.Setup(um => um.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _authService.LoginAsync(userLoginDto);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Token.Should().Be("new_jwt_token");
            result.RefreshToken.Should().Be("new_refresh_token");
            user.RefreshTokens.Should().Contain(newRefreshToken);
            _mockUserManager.Verify(um => um.UpdateAsync(user), Times.Once); // Verify new token was saved
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnFailure_WhenUserNotFound()
        {
            // Arrange
            var userLoginDto = new UserLoginDto { Email = "nonexistent@example.com", Password = "Password123!" };

            var users = new List<AppUser>().AsQueryable(); // Empty list
            var asyncUsers = new TestAsyncEnumerable<AppUser>(users);

            _mockUserManager.Setup(um => um.Users).Returns(asyncUsers);

            // Act
            var result = await _authService.LoginAsync(userLoginDto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Messages.Should().Contain("Email or Password is incorrect");
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnFailure_WhenEmailIsNotConfirmed()
        {
            // Arrange
            var userLoginDto = new UserLoginDto { Email = "unconfirmed@example.com", Password = "Password123!" };
            var user = new AppUser { Email = userLoginDto.Email, EmailConfirmed = false };

            var users = new List<AppUser> { user }.AsQueryable();
            var asyncUsers = new TestAsyncEnumerable<AppUser>(users);

            _mockUserManager.Setup(um => um.Users).Returns(asyncUsers);

            // Act
            var result = await _authService.LoginAsync(userLoginDto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Messages.Should().Contain("Email needs to be Confirmed");
        }

        [Fact]
        public async Task LoginAsync_ShouldReturnFailure_WhenPasswordIsIncorrect()
        {
            // Arrange
            var userLoginDto = new UserLoginDto { Email = "test@example.com", Password = "WrongPassword" };
            var user = new AppUser { Email = userLoginDto.Email, EmailConfirmed = true };

            var users = new List<AppUser> { user }.AsQueryable();
            var asyncUsers = new TestAsyncEnumerable<AppUser>(users);

            _mockUserManager.Setup(um => um.Users).Returns(asyncUsers);
            _mockUserManager.Setup(um => um.CheckPasswordAsync(user, userLoginDto.Password)).ReturnsAsync(false);

            // Act
            var result = await _authService.LoginAsync(userLoginDto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Messages.Should().Contain("Email or Password is incorrect");
        }

        #endregion

    }


}
