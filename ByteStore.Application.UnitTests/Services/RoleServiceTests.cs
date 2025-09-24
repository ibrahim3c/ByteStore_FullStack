using ByteStore.Domain.Abstractions.Result;
using ByteStore.Domain.Entities;
using BytStore.Application.DTOs.Identity;
using BytStore.Application.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace ByteStore.Application.UnitTests.Services
{

    public class RoleServiceTests
    {
        private readonly Mock<RoleManager<AppRole>> _mockRoleManager;
        private readonly RoleService _roleService;

        public RoleServiceTests()
        {
            // RoleManager requires an IRoleStore mock in its constructor
            var roleStoreMock = new Mock<IRoleStore<AppRole>>();
            _mockRoleManager = new Mock<RoleManager<AppRole>>(
                roleStoreMock.Object, null, null, null, null);

            _roleService = new RoleService(_mockRoleManager.Object);
        }

        #region GetAllRolesAsync Tests

        [Fact]
        public async Task GetAllRolesAsync_ShouldReturnSuccessWithRoles_WhenRolesExist()
        {
            // Arrange
            var roles = new List<AppRole>
        {
            new AppRole { Id = "1", Name = "Admin" },
            new AppRole { Id = "2", Name = "User" }
        };

            var asyncRoles = new TestAsyncEnumerable<AppRole>(roles);
            _mockRoleManager.Setup(rm => rm.Roles).Returns(asyncRoles);

            // Act
            var result = await _roleService.GetAllRolesAsync();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().HaveCount(2);
            result.Value.First().RoleName.Should().Be("Admin");
        }

        [Fact]
        public async Task GetAllRolesAsync_ShouldReturnFailure_WhenNoRolesExist()
        {
            // Arrange
            var roles = new List<AppRole>();
            var asyncRoles = new TestAsyncEnumerable<AppRole>(roles);
            _mockRoleManager.Setup(rm => rm.Roles).Returns(asyncRoles);

            // Act
            var result = await _roleService.GetAllRolesAsync();

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(RoleErrors.NotFound);
        }

        #endregion

        #region GetRoleByIdAsync Tests

        [Fact]
        public async Task GetRoleByIdAsync_ShouldReturnSuccess_WhenRoleExists()
        {
            // Arrange
            var roleId = "1";
            var role = new AppRole { Id = roleId, Name = "Admin" };
            _mockRoleManager.Setup(rm => rm.FindByIdAsync(roleId)).ReturnsAsync(role);

            // Act
            var result = await _roleService.GetRoleByIdAsync(roleId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.RoleId.Should().Be(roleId);
            result.Value.RoleName.Should().Be("Admin");
        }

        [Fact]
        public async Task GetRoleByIdAsync_ShouldReturnFailure_WhenRoleNotFound()
        {
            // Arrange
            _mockRoleManager.Setup(rm => rm.FindByIdAsync(It.IsAny<string>())).ReturnsAsync((AppRole)null);

            // Act
            var result = await _roleService.GetRoleByIdAsync("99");

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(RoleErrors.NotFound);
        }

        #endregion

        #region AddRoleAsync Tests

        [Fact]
        public async Task AddRoleAsync_ShouldReturnSuccess_WhenRoleIsCreated()
        {
            // Arrange
            var roleDto = new GetRoleDto { RoleName = "NewRole" };
            _mockRoleManager.Setup(rm => rm.FindByNameAsync(roleDto.RoleName)).ReturnsAsync((AppRole)null);
            _mockRoleManager.Setup(rm => rm.CreateAsync(It.Is<AppRole>(r => r.Name == roleDto.RoleName)))
                            .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _roleService.AddRoleAsync(roleDto);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task AddRoleAsync_ShouldReturnFailure_WhenRoleAlreadyExists()
        {
            // Arrange
            var roleDto = new GetRoleDto { RoleName = "ExistingRole" };
            _mockRoleManager.Setup(rm => rm.FindByNameAsync(roleDto.RoleName)).ReturnsAsync(new AppRole());

            // Act
            var result = await _roleService.AddRoleAsync(roleDto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(RoleErrors.AlreadyExists);
        }

        #endregion

        #region UpdateRoleAsync Tests

        [Fact]
        public async Task UpdateRoleAsync_ShouldReturnSuccess_WhenRoleIsUpdated()
        {
            // Arrange
            var roleDto = new GetRoleDto { RoleId = "1", RoleName = "UpdatedName" };
            var existingRole = new AppRole { Id = "1", Name = "OldName" };
            _mockRoleManager.Setup(rm => rm.FindByIdAsync(roleDto.RoleId)).ReturnsAsync(existingRole);
            _mockRoleManager.Setup(rm => rm.UpdateAsync(existingRole)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _roleService.UpdateRoleAsync(roleDto);

            // Assert
            result.IsSuccess.Should().BeTrue();
            existingRole.Name.Should().Be("UpdatedName");
        }

        [Fact]
        public async Task UpdateRoleAsync_ShouldReturnFailure_WhenRoleNotFound()
        {
            // Arrange
            var roleDto = new GetRoleDto { RoleId = "99" };
            _mockRoleManager.Setup(rm => rm.FindByIdAsync(roleDto.RoleId)).ReturnsAsync((AppRole)null);

            // Act
            var result = await _roleService.UpdateRoleAsync(roleDto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(RoleErrors.NotFound);
        }

        #endregion

        #region DeleteRoleAsync Tests

        [Fact]
        public async Task DeleteRoleAsync_ShouldReturnSuccess_WhenRoleIsDeleted()
        {
            // Arrange
            var roleId = "1";
            var role = new AppRole { Id = roleId, Name = "ToDelete" };
            _mockRoleManager.Setup(rm => rm.FindByIdAsync(roleId)).ReturnsAsync(role);
            _mockRoleManager.Setup(rm => rm.DeleteAsync(role)).ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _roleService.DeleteRoleAsync(roleId);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteRoleAsync_ShouldReturnFailure_WhenRoleNotFound()
        {
            // Arrange
            var roleId = "99";
            _mockRoleManager.Setup(rm => rm.FindByIdAsync(roleId)).ReturnsAsync((AppRole)null);

            // Act
            var result = await _roleService.DeleteRoleAsync(roleId);

                
            // Assert
        result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(RoleErrors.NotFound);
        }

        #endregion
    }


}