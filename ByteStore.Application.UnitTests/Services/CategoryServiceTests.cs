using ByteStore.Domain.Abstractions.Result;
using ByteStore.Domain.Entities;
using ByteStore.Domain.Repositories;
using BytStore.Application.DTOs.Category;
using BytStore.Application.Services;
using FluentAssertions;
using Moq;

namespace ByteStore.Application.UnitTests.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly CategoryService _categoryService;

        public CategoryServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();

            // Setup the mock UnitOfWork to return our mock CategoryRepository
            _mockUnitOfWork.Setup(uow => uow.CategoryRepository).Returns(_mockCategoryRepository.Object);

            // Instantiate the service with the mocked dependency
            _categoryService = new CategoryService(_mockUnitOfWork.Object);
        }

        #region GetAllCategoriesAsync Tests

        [Fact]
        public async Task GetAllCategoriesAsync_ShouldReturnSuccessWithCategoryDtos_WhenCategoriesExist()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Electronics", Description = "Gadgets and devices" },
                new Category { Id = 2, Name = "Books", Description = "Paper and digital books" }
            };
            _mockCategoryRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(categories);

            // Act
            var result = await _categoryService.GetAllCategoriesAsync();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Should().HaveCount(2);
            result.Value.First().Name.Should().Be("Electronics");
        }

        #endregion

        #region GetCategoryByIdAsync Tests

        [Fact]
        public async Task GetCategoryByIdAsync_ShouldReturnSuccessWithCategoryDto_WhenCategoryExists()
        {
            // Arrange
            int categoryId = 1;
            var category = new Category { Id = categoryId, Name = "Laptops", Description = "Portable computers" };
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);

            // Act
            var result = await _categoryService.GetCategoryByIdAsync(categoryId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Id.Should().Be(categoryId);
            result.Value.Name.Should().Be("Laptops");
        }

        [Fact]
        public async Task GetCategoryByIdAsync_ShouldReturnFailure_WhenCategoryDoesNotExist()
        {
            // Arrange
            int categoryId = 99;
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync((Category)null);

            // Act
            var result = await _categoryService.GetCategoryByIdAsync(categoryId);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(CategoryErrors.CategoryNotFound);
        }

        #endregion

        #region CreateCategoryAsync Tests

        [Fact]
        public async Task CreateCategoryAsync_ShouldReturnSuccess_AndCallRepositoryMethods()
        {
            // Arrange
            var categoryDto = new CategoryDto { Name = "NewCategory", Description = "A new description" };

            // Act
            var result = await _categoryService.CreateCategoryAsync(categoryDto);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mockCategoryRepository.Verify(repo => repo.AddAsync(It.Is<Category>(c => c.Name == categoryDto.Name)), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        #endregion

        #region UpdateCategoryAsync Tests

        [Fact]
        public async Task UpdateCategoryAsync_ShouldReturnSuccess_WhenCategoryExists()
        {
            // Arrange
            int categoryId = 1;
            var existingCategory = new Category { Id = categoryId, Name = "OldName", Description = "OldDesc" };
            var updatedDto = new CategoryDto { Name = "NewName", Description = "NewDesc" };

            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(existingCategory);

            // Act
            var result = await _categoryService.UpdateCategoryAsync(categoryId, updatedDto);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mockCategoryRepository.Verify(repo => repo.Update(It.Is<Category>(c => c.Name == updatedDto.Name)), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateCategoryAsync_ShouldReturnFailure_WhenCategoryDoesNotExist()
        {
            // Arrange
            int categoryId = 99;
            var updatedDto = new CategoryDto { Name = "NewName" };
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync((Category)null);

            // Act
            var result = await _categoryService.UpdateCategoryAsync(categoryId, updatedDto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(CategoryErrors.CategoryNotFound);
        }

        #endregion

        #region DeleteCategoryAsync Tests

        [Fact]
        public async Task DeleteCategoryAsync_ShouldReturnSuccess_WhenCategoryExists()
        {
            // Arrange
            int categoryId = 1;
            var existingCategory = new Category { Id = categoryId, Name = "ToBeDeleted" };
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(existingCategory);

            // Act
            var result = await _categoryService.DeleteCategoryAsync(categoryId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mockCategoryRepository.Verify(repo => repo.Delete(existingCategory), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteCategoryAsync_ShouldReturnFailure_WhenCategoryDoesNotExist()
        {
            // Arrange
            int categoryId = 99;
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync((Category)null);

            // Act
            var result = await _categoryService.DeleteCategoryAsync(categoryId);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(CategoryErrors.CategoryNotFound);
        }

        #endregion
    }
}
