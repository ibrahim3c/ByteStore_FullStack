using ByteStore.Domain.Abstractions.Constants;
using ByteStore.Domain.Abstractions.Result;
using ByteStore.Domain.Entities;
using BytStore.Application.DTOs.Identity;
using BytStore.Application.IServices;
using BytStore.Application.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace ByteStore.Application.UnitTests.Services
{
    public class UserServiceTests
    {
        private readonly Mock<UserManager<AppUser>> _mockUserManager;
        private readonly Mock<RoleManager<AppRole>> _mockRoleManager;
        private readonly Mock<IRolesService> _mockRolesService;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            // Mock UserManager
            var userStoreMock = new Mock<IUserStore<AppUser>>();
            _mockUserManager = new Mock<UserManager<AppUser>>(userStoreMock.Object, null, null, null, null, null, null, null, null);

            // Mock RoleManager
            var roleStoreMock = new Mock<IRoleStore<AppRole>>();
            _mockRoleManager = new Mock<RoleManager<AppRole>>(roleStoreMock.Object, null, null, null, null);

            // Mock IRolesService
            _mockRolesService = new Mock<IRolesService>();

            _userService = new UserService(_mockUserManager.Object, _mockRoleManager.Object, _mockRolesService.Object);
        }

        #region Get User Information

        [Fact]
        public async Task GetAllUsersAsync_ShouldReturnSuccessWithUsers_WhenUsersExist()
        {
            // Arrange  
            var users = new List<AppUser> { new AppUser { UserName = "testuser" } };
            var asyncUsers = new TestAsyncEnumerable<AppUser>(users);

            _mockUserManager.Setup(um => um.Users).Returns(asyncUsers);

            // Act
            var result = await _userService.GetAllUsersAsync();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().HaveCount(1);
        }

        [Fact]
        public async Task GetUserByIdAsync_ShouldReturnFailure_WhenUserNotFound()
        {
            // Arrange
            _mockUserManager.Setup(um => um.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((AppUser)null);

            // Act
            var result = await _userService.GetUserByIdAsync("nonexistent-id");

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(UserErrors.NotFound);
        }

        #endregion

        #region User Account Management

        [Fact]
        public async Task AddUserAsync_ShouldReturnSuccess_WhenCreationSucceeds()
        {
            // Arrange
            var userDto = new CreatedUserDto { Email = "new@example.com", Password = "Password123" };
            _mockUserManager.Setup(um => um.FindByEmailAsync(userDto.Email)).ReturnsAsync((AppUser)null);
            _mockUserManager.Setup(um => um.CreateAsync(It.IsAny<AppUser>(), userDto.Password))
                            .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _userService.AddUserAsync(userDto);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnSuccess_WhenDeletionSucceeds()
        {
            // Arrange
            var user = new AppUser();
            _mockUserManager.Setup(um => um.FindByIdAsync("existing-id")).ReturnsAsync(user);
            _mockUserManager.Setup(um => um.DeleteAsync(user)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _userService.DeleteUserAsync("existing-id");

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldReturnFailure_WhenUserNotFound()
        {
            // Arrange
            _mockUserManager.Setup(um => um.FindByIdAsync("nonexistent-id")).ReturnsAsync((AppUser)null);

            // Act
            var result = await _userService.DeleteUserAsync("nonexistent-id");

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(UserErrors.NotFound);
        }

        #endregion

        #region User Role Management

        [Fact]
        public async Task ManageUserRolesAsync_ShouldAddAndRemoveRolesCorrectly()
        {
            // Arrange
            var userId = "test-user-id";
            var user = new AppUser { Id = userId };
            var manageDto = new ManageRolesDto
            {
                UserId = userId,
                Roles = new List<RolesDto>
                {
                    new RolesDto { RoleName = "Admin", IsSelected = true }, // Add this role
                    new RolesDto { RoleName = "User", IsSelected = false } // Remove this role
                }
            };

            _mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            _mockUserManager.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" }); // User currently has "User" role

            // Act
            var result = await _userService.ManageUserRolesAsync(manageDto);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mockUserManager.Verify(um => um.AddToRoleAsync(user, "Admin"), Times.Once); // Should be called to add Admin
            _mockUserManager.Verify(um => um.RemoveFromRoleAsync(user, "User"), Times.Once); // Should be called to remove User
        }

        [Fact]
        public async Task GetRolesOfUserAsync_ShouldReturnSuccessWithRoles_WhenUserAndRolesExist()
        {
            // Arrange
            var userId = "test-user-id";
            var user = new AppUser { Id = userId };
            var roleNames = new List<string> { "Admin" };
            var role = new AppRole { Id = "1", Name = "Admin" };

            _mockUserManager.Setup(um => um.FindByIdAsync(userId)).ReturnsAsync(user);
            _mockUserManager.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(roleNames);
            _mockRoleManager.Setup(rm => rm.FindByNameAsync("Admin")).ReturnsAsync(role);

            // Act
            var result = await _userService.GetRolesOfUserAsync(userId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().HaveCount(1);
            result.Value.First().RoleName.Should().Be("Admin");
        }

        #endregion
    }
}
