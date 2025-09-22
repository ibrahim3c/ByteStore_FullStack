using ByteStore.Domain.Abstractions.Result;
using ByteStore.Domain.Entities;
using ByteStore.Domain.Repositories;
using BytStore.Application.DTOs.Brand;
using BytStore.Application.Services;
using FluentAssertions;
using Moq;

namespace ByteStore.Application.UnitTests.Services
{
    public class BrandServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IBaseRepository<Brand>> _mockBrandRepository;
        private readonly BrandService _brandService;

        public BrandServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockBrandRepository = new Mock<IBaseRepository<Brand>>();

            // Setup the mock UnitOfWork to return our mock repository when GetRepository<Brand>() is called
            _mockUnitOfWork.Setup(uow => uow.GetRepository<Brand>()).Returns(_mockBrandRepository.Object);

            // Instantiate the service with the mocked dependency
            _brandService = new BrandService(_mockUnitOfWork.Object);
        }

        #region GetAllBrandsAsync Tests

        [Fact]
        public async Task GetAllBrandsAsync_ShouldReturnSuccessWithBrandDtos_WhenBrandsExist()
        {
            // Arrange
            var brands = new List<Brand>
            {
                new Brand { Id = 1, Name = "Apple", Description = "Electronics" },
                new Brand { Id = 2, Name = "Samsung", Description = "Electronics" }
            };
            _mockBrandRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(brands);

            // Act
            var result = await _brandService.GetAllBrandsAsync();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Should().HaveCount(2);
            result.Value.First().Name.Should().Be("Apple");
        }

        [Fact]
        public async Task GetAllBrandsAsync_ShouldReturnSuccessWithEmptyList_WhenNoBrandsExist()
        {
            // Arrange
            _mockBrandRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(new List<Brand>());

            // Act
            var result = await _brandService.GetAllBrandsAsync();

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Should().BeEmpty();
        }

        #endregion

        #region GetBrandByIdAsync Tests

        [Fact]
        public async Task GetBrandByIdAsync_ShouldReturnSuccessWithBrandDto_WhenBrandExists()
        {
            // Arrange
            int brandId = 1;
            var brand = new Brand { Id = brandId, Name = "Sony", Description = "Entertainment" };
            _mockBrandRepository.Setup(repo => repo.GetByIdAsync(brandId)).ReturnsAsync(brand);

            // Act
            var result = await _brandService.GetBrandByIdAsync(brandId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.Id.Should().Be(brandId);
            result.Value.Name.Should().Be("Sony");
        }

        [Fact]
        public async Task GetBrandByIdAsync_ShouldReturnFailure_WhenBrandDoesNotExist()
        {
            // Arrange
            int brandId = 99;
            _mockBrandRepository.Setup(repo => repo.GetByIdAsync(brandId)).ReturnsAsync((Brand)null);

            // Act
            var result = await _brandService.GetBrandByIdAsync(brandId);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(BrandErrors.BrandNotFound);
        }

        #endregion

        #region CreateBrandAsync Tests

        [Fact]
        public async Task CreateBrandAsync_ShouldReturnSuccess_AndCallRepositoryMethods()
        {
            // Arrange
            var brandDto = new BrandDto { Name = "NewBrand", Description = "New Description" };

            // Act
            var result = await _brandService.CreateBrandAsync(brandDto);

            // Assert
            result.IsSuccess.Should().BeTrue();

            // Verify that the repository's AddAsync and SaveChangesAsync methods were called exactly once
            _mockBrandRepository.Verify(repo => repo.AddAsync(It.Is<Brand>(b => b.Name == brandDto.Name)), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        #endregion

        #region UpdateBrandAsync Tests

        [Fact]
        public async Task UpdateBrandAsync_ShouldReturnSuccess_WhenBrandExists()
        {
            // Arrange
            int brandId = 1;
            var existingBrand = new Brand { Id = brandId, Name = "OldName", Description = "OldDesc" };
            var updatedDto = new BrandDto { Name = "NewName", Description = "NewDesc" };

            _mockBrandRepository.Setup(repo => repo.GetByIdAsync(brandId)).ReturnsAsync(existingBrand);

            // Act
            var result = await _brandService.UpdateBrandAsync(brandId, updatedDto);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mockBrandRepository.Verify(repo => repo.Update(It.Is<Brand>(b => b.Name == updatedDto.Name)), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateBrandAsync_ShouldReturnFailure_WhenBrandDoesNotExist()
        {
            // Arrange
            int brandId = 99;
            var updatedDto = new BrandDto { Name = "NewName", Description = "NewDesc" };
            _mockBrandRepository.Setup(repo => repo.GetByIdAsync(brandId)).ReturnsAsync((Brand)null);

            // Act
            var result = await _brandService.UpdateBrandAsync(brandId, updatedDto);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(BrandErrors.BrandNotFound);
        }

        #endregion

        #region DeleteBrandAsync Tests

        [Fact]
        public async Task DeleteBrandAsync_ShouldReturnSuccess_WhenBrandExists()
        {
            // Arrange
            int brandId = 1;
            var existingBrand = new Brand { Id = brandId, Name = "ToBeDeleted" };
            _mockBrandRepository.Setup(repo => repo.GetByIdAsync(brandId)).ReturnsAsync(existingBrand);

            // Act
            var result = await _brandService.DeleteBrandAsync(brandId);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _mockBrandRepository.Verify(repo => repo.Delete(existingBrand), Times.Once);
            _mockUnitOfWork.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteBrandAsync_ShouldReturnFailure_WhenBrandDoesNotExist()
        {
            // Arrange
            int brandId = 99;
            _mockBrandRepository.Setup(repo => repo.GetByIdAsync(brandId)).ReturnsAsync((Brand)null);

            // Act
            var result = await _brandService.DeleteBrandAsync(brandId);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be(BrandErrors.BrandNotFound);
        }

        #endregion
    }
}
