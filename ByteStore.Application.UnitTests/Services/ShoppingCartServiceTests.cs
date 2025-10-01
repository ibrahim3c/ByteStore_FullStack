using ByteStore.Domain.Abstractions.Result;
using ByteStore.Domain.Entities;
using ByteStore.Domain.Repositories;
using BytStore.Application.DTOs.ShoppingCart;
using BytStore.Application.Services;
using FluentAssertions;
using Moq;

namespace ByteStore.Application.UnitTests.Services
{
    public class ShoppingCartServiceTests
    {
        private readonly Mock<IShoppingCartRepository> _mockShoppingCartRepository;
        private readonly ShoppingCartService _shoppingCartService;

        public ShoppingCartServiceTests()
        {
            _mockShoppingCartRepository = new Mock<IShoppingCartRepository>();
            _shoppingCartService = new ShoppingCartService(_mockShoppingCartRepository.Object);
        }

        #region GetCartAsync Tests

        [Fact]
        public async Task GetCartAsync_ShouldReturnSuccessWithCartDto_WhenCartExists()
        {
            // Arrange
            var customerId = "test-customer";
            var shoppingCart = new ShoppingCart()
            {
                CustomerId=customerId,
                CartItems = new List<CartItem>
                {
                    new CartItem { ProductId = 1, Name = "Laptop", Quantity = 1 }
                }
            };
            _mockShoppingCartRepository.Setup(repo => repo.GetCartAsync(customerId)).ReturnsAsync(shoppingCart);

            // Act
            var result = await _shoppingCartService.GetCartAsync(customerId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.CustomerId.Should().Be(customerId);
            result.Value.CartItems.Should().HaveCount(1);
            result.Value.CartItems.First().Name.Should().Be("Laptop");
        }

        [Fact]
        public async Task GetCartAsync_ShouldReturnFailure_WhenCartDoesNotExist()
        {
            // Arrange
            _mockShoppingCartRepository.Setup(repo => repo.GetCartAsync(It.IsAny<string>())).ReturnsAsync((ShoppingCart)null);

            // Act
            var result = await _shoppingCartService.GetCartAsync("nonexistent-customer");

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(CartErrors.NotFound);
        }

        #endregion

        #region SaveCartAsync Tests

        [Fact]
        public async Task SaveCartAsync_ShouldReturnSuccess_WhenRepositorySavesSuccessfully()
        {
            // Arrange
            var cartDto = new ShoppingCartDto
            {
                CustomerId = "test-customer",
                CartItems = new List<CartItemDto> { new CartItemDto { ProductId = 1, Quantity = 2 } }
            };
            _mockShoppingCartRepository.Setup(repo => repo.SaveCartAsync(It.IsAny<ShoppingCart>())).ReturnsAsync(true);

            // Act
            var result = await _shoppingCartService.SaveCartAsync(cartDto);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mockShoppingCartRepository.Verify(repo => repo.SaveCartAsync(
                It.Is<ShoppingCart>(c => c.CustomerId == cartDto.CustomerId && c.CartItems.First().Quantity == 2)), Times.Once);
        }

        [Fact]
        public async Task SaveCartAsync_ShouldReturnFailure_WhenRepositoryFailsToSave()
        {
            // Arrange
            var cartDto = new ShoppingCartDto();
            _mockShoppingCartRepository.Setup(repo => repo.SaveCartAsync(It.IsAny<ShoppingCart>())).ReturnsAsync(false);

            // Act
            var result = await _shoppingCartService.SaveCartAsync(cartDto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(CartErrors.SaveFailed);
        }

        #endregion

        #region ClearCartAsync Tests

        [Fact]
        public async Task ClearCartAsync_ShouldReturnSuccess_WhenRepositoryClearsSuccessfully()
        {
            // Arrange
            var customerId = "test-customer";
            _mockShoppingCartRepository.Setup(repo => repo.ClearCartAsync(customerId)).ReturnsAsync(true);

            // Act
            var result = await _shoppingCartService.ClearCartAsync(customerId);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task ClearCartAsync_ShouldReturnFailure_WhenRepositoryFailsToClear()
        {
            // Arrange
            var customerId = "test-customer";
            _mockShoppingCartRepository.Setup(repo => repo.ClearCartAsync(customerId)).ReturnsAsync(false);

            // Act
            var result = await _shoppingCartService.ClearCartAsync(customerId);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(CartErrors.ClearFailed);
        }

        #endregion
    }
}
